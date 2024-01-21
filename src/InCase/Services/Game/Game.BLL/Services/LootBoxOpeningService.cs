using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Game.BLL.Services;

public class LootBoxOpeningService(
    ILogger<LootBoxOpeningService> logger, 
    ApplicationDbContext context, 
    BasePublisher publisher) : ILootBoxOpeningService
{
    public async Task<GameItemResponse> OpenBox(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        var box = await context.Boxes
            .Include(lb => lb.Inventories!)
            .ThenInclude(lbi => lbi.Item)
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        var info = await context.AdditionalInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        var promo = await context.UserPromocodes
            .AsNoTracking()
            .FirstOrDefaultAsync(uhp => uhp.UserId == userId, cancellation);

        if (box.IsLocked) throw new ForbiddenException("Кейс заблокирован");
        if (info.Balance < box.Cost) throw new PaymentRequiredException("Недостаточно средств");
        if (promo is not null)
        {
            await publisher.SendAsync(new UserPromocodeBackTemplate
            {
                Id = promo.Id,
            }, cancellation);

            context.UserPromocodes.Remove(promo);
        }

        var path = await context.PathBanners
            .AsNoTracking()
            .FirstOrDefaultAsync(upb => upb.BoxId == box.Id && upb.UserId == userId, cancellation);

        var discount = promo?.Discount ?? 0;
        var boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

        info.Balance -= boxCost;
        box.Balance += boxCost;

        var winItem = OpenLootBoxService.RandomizeBySmallest(in box);
        var revenue = OpenLootBoxService.GetRevenue(box.Cost);
        var expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

        if (path is not null && 
            box.ExpirationBannerDate is not null && 
            box.ExpirationBannerDate >= DateTime.UtcNow)
        {
            --path.NumberSteps;

            var retention = OpenLootBoxService.GetRetentionBanner(box.Cost);
            expenses = winItem.Cost + retention;

            if (path.NumberSteps == 0)
            {
                winItem = box.Inventories?
                              .FirstOrDefault(f => f.ItemId == path.ItemId)?.Item ??
                          await context.Items
                              .AsNoTracking()
                              .FirstOrDefaultAsync(i => i.Id == path.ItemId, cancellation);

                winItem!.Cost = path.FixedCost;
                expenses = retention;

                context.PathBanners.Remove(path);
            }
            else
            {
                revenue = 0;

                context.PathBanners.Attach(path);
                context.Entry(path).Property(p => p.NumberSteps).IsModified = true;
            }
        }

        var opening = new UserOpening
        {
            UserId = userId,
            BoxId = box.Id,
            ItemId = winItem.Id,
            Date = DateTime.UtcNow
        };

        box.Balance -= expenses;

        logger.LogTrace($"UID - {userId}, открыл - {id}, выпало - {winItem.Id}, сняло - {boxCost}");

        context.Entry(info).Property(p => p.Balance).IsModified = true;
        context.Entry(box).Property(p => p.Balance).IsModified = true;
        await context.Openings.AddAsync(opening, cancellation);
        await context.SaveChangesAsync(cancellation);

        logger.LogTrace($"UID - {userId}, UOID - {opening.Id} зафиксировал");

        await publisher.SendAsync(new UserInventoryTemplate
        {
            Date = DateTime.UtcNow,
            FixedCost = winItem.Cost,
            ItemId = winItem.Id,
            UserId = userId,
        }, cancellation);
        await publisher.SendAsync(new SiteStatisticsTemplate { LootBoxes = 1 }, cancellation);
        await publisher.SendAsync(new SiteStatisticsAdminTemplate
        {
            RevenueLootBoxCommission = revenue,
            FundsUsersInventories = winItem.Cost
        }, cancellation);

        return new GameItemResponse
        {
            Id = winItem.Id,
            Cost = winItem.Cost,
        };
    }

    public async Task<GameItemResponse> OpenVirtualBox(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        var userInfo = await context.AdditionalInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        var box = await context.Boxes
            .Include(lb => lb.Inventories!)
                .ThenInclude(lbi => lbi.Item)
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        if (!userInfo.IsGuestMode) throw new ForbiddenException("Не включен режим гостя");

        box.VirtualBalance += box.Cost;

        var winItem = OpenLootBoxService.RandomizeBySmallest(in box, true);
        var revenue = OpenLootBoxService.GetRevenue(box.Cost);
        var expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

        box.VirtualBalance -= expenses;

        context.Boxes.Attach(box);
        context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;
        await context.SaveChangesAsync(cancellation);

        return new GameItemResponse
        {
            Id = winItem.Id,
            Cost = winItem.Cost,
        };
    }


    public async Task<List<GameItemBigOpenResponse>> OpenVirtualBox(
        Guid userId,
        Guid id, 
        int count, 
        bool isAdmin = false,
        CancellationToken cancellation = default)
    {
        if (count < 1 || (count > 100 && !isAdmin))
            throw new BadRequestException("Количество открытий должно быть в диапазоне от 1 до 100");

        var userInfo = await context.AdditionalInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(uai => uai.UserId == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        var box = await context.Boxes
            .Include(lb => lb.Inventories!)
                .ThenInclude(lbi => lbi.Item)
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
            throw new NotFoundException("Кейс не найден");

        if (!userInfo.IsGuestMode) throw new ForbiddenException("Не включен режим гостя");

        context.Boxes.Attach(box);
        context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;

        var winItems = new List<GameItemBigOpenResponse>();

        for (var i = 0; i < count; i++)
        {
            try
            {
                box.VirtualBalance += box.Cost;

                var item = OpenLootBoxService.RandomizeBySmallest(in box, true);
                var revenue = OpenLootBoxService.GetRevenue(box.Cost);
                var expenses = OpenLootBoxService.GetExpenses(item.Cost, revenue);

                box.VirtualBalance -= expenses;

                var index = winItems.FindIndex(gi => gi.Id == item.Id);

                if (index != -1) winItems[index].Count++;
                else winItems.Add(new GameItemBigOpenResponse 
                { 
                    Id = item.Id, 
                    Cost = item.Cost, 
                    Count = 1 
                });

                await context.SaveChangesAsync(cancellation);
            }
            catch(Exception ex)
            {
                await context.SaveChangesAsync(cancellation);

                throw new StatusCodeExtendedException(ErrorCodes.UnknownError, ex.Message, winItems);
            }
        }

        return winItems;
    }
}