using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.MassTransit;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services;
public class UserInventoryService(
    ApplicationDbContext context, 
    IWithdrawItemService withdrawService, 
    BasePublisher publisher) : IUserInventoryService
{
    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public async Task<List<UserInventoryResponse>> GetAsync(Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId,cancellation))
            throw new NotFoundException("Пользователь не найден");

        var inventories = await context.UserInventories
            .AsNoTracking()
            .Where(ui => ui.UserId == userId)
            .ToListAsync(cancellation);

        return inventories.ToResponse();
    }

    public async Task<List<UserInventoryResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var inventories = await context.UserInventories
            .AsNoTracking()
            .Where(ui => ui.UserId == userId)
            .OrderByDescending(ui => ui.Date)
            .Take(count)
            .ToListAsync(cancellation);

        return inventories.ToResponse();
    }

    public async Task<UserInventory?> GetByConsumerAsync(Guid id, CancellationToken cancellation = default) => 
        await context.UserInventories
        .AsNoTracking()
        .FirstOrDefaultAsync(ui => ui.Id == id, cancellation);

    public async Task<UserInventoryResponse> GetByIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var inventory = await context.UserInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(ui => ui.Id == id, cancellation) ?? 
            throw new NotFoundException("Инвентарь не найден");

        return inventory.ToResponse();
    }

    public async Task<UserInventoryResponse> CreateAsync(UserInventoryTemplate template, CancellationToken cancellation = default)
    {
        if (!await context.GameItems.AnyAsync(gi => gi.Id == template.ItemId, cancellation))
            throw new NotFoundException("Предмет не найден");
        if (!await context.Users.AnyAsync(u => u.Id == template.UserId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var inventory = new UserInventory
        {
            Id = template.Id,
            Date = template.Date,
            FixedCost = template.FixedCost,
            ItemId = template.ItemId,
            UserId = template.UserId
        };

        await context.UserInventories.AddAsync(inventory, cancellation);
        await context.SaveChangesAsync(cancellation);

        return inventory.ToResponse();
    }

    public async Task<List<UserInventoryResponse>> ExchangeAsync(ExchangeItemRequest request, Guid userId, CancellationToken cancellation = default)
    {
        if (request.Items is null)
            throw new BadRequestException("Не выбран ни один предмет для обмена");

        if (request.Items.Sum(x => x.Count) > 10)
            throw new BadRequestException("Запрещено выводить более 10 предметов");

        var inventory = await context.UserInventories
            .Include(ui => ui.Item)
            .Include(ui => ui.Item!.Game!)
                .ThenInclude(g => g.Market)
            .FirstOrDefaultAsync(ui => ui.Id == request.InventoryId && ui.UserId == userId, cancellation) ??
            throw new NotFoundException("Заменяемый предмет не найден в инвентаре");

        var itemInfo = await withdrawService
            .GetItemInfoAsync(inventory.Item!, cancellation);

        if (itemInfo.Count != 0 && itemInfo.PriceKopecks != 0)
        {
            var price = itemInfo.PriceKopecks * 0.01M;
            var itemCost = inventory.FixedCost / 7;

            if (price >= itemCost * 0.9M && price <= itemCost * 1.1M)
                throw new ConflictException("Товар может быть обменен только в случае нестабильности цены");
        }

        var inventories = new List<UserInventory>();

        decimal totalItemsCost = 0;

        foreach (var itemModel in request.Items)
        {
            var gameItem = await context.GameItems
                               .AsNoTracking()
                               .FirstOrDefaultAsync(gi => gi.Id == itemModel.ItemId, cancellation) ?? 
                           throw new NotFoundException($"Предмет с заданным Id {itemModel.ItemId} не найден");

            totalItemsCost += gameItem.Cost * itemModel.Count;

            for (var i = 0; i < itemModel.Count; i++)
            {
                inventories.Add(new UserInventory
                {
                    FixedCost = gameItem.Cost,
                    Date = inventory.Date,
                    ItemId = gameItem.Id,
                    UserId = userId
                });
            }
        }

        var differenceCost = inventory.FixedCost - totalItemsCost;

        if (differenceCost < 0) throw new BadRequestException("Стоимость товара при обмене не может быть выше");

        context.UserInventories.Remove(inventory);
        await context.UserInventories.AddRangeAsync(inventories, cancellation);
        await context.SaveChangesAsync(cancellation);

        foreach (var template in inventories.Select(userInventory => userInventory.ToTemplate()))
        {
            template.FixedCost = differenceCost;
            await publisher.SendAsync(template, cancellation);
        }

        await publisher.SendAsync(new SiteStatisticsAdminTemplate { FundsUsersInventories = -differenceCost * request.Items.Count }, cancellation);

        _logger.Log(NLog.LogLevel.Info, "Exchanged: UserId - {0}," +
                                       " GameItemId - {1}, Game - {2}",
                                       inventory.UserId, inventory.ItemId,
                                       inventory.Item?.Game?.Name);

        return inventories.ToResponse();
    }
    public async Task<SellItemResponse> SellAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(uai => uai.Id == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        var inventory = await context.UserInventories
            .AsNoTracking()
            .Include(userInventory => userInventory.Item)
            .ThenInclude(gameItem => gameItem!.Game)
            .FirstOrDefaultAsync(ui => ui.Id == id && ui.UserId == userId, cancellation) ??
            throw new NotFoundException("Предмет не найден в инвентаре");

        _logger.Log(NLog.LogLevel.Info, "Selled: UserId - {0}, GameItemId - {1}, Game - {2}",
            user.Id, inventory.ItemId, inventory.Item?.Game?.Name);

        context.UserInventories.Remove(inventory);
        await context.SaveChangesAsync(cancellation);

        await publisher.SendAsync(inventory.ToTemplate(), cancellation);
        await publisher.SendAsync(new SiteStatisticsAdminTemplate { FundsUsersInventories = -inventory.FixedCost }, cancellation);

        return new SellItemResponse { Cost = inventory.FixedCost };
    }

    public async Task<SellItemResponse> SellLastAsync(Guid itemId, Guid userId, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(uai => uai.Id == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");
        
        var inventories = await context.UserInventories
            .AsNoTracking()
            .Where(ui => ui.UserId == userId && ui.ItemId == itemId).Include(userInventory => userInventory.Item)
            .ThenInclude(gameItem => gameItem!.Game)
            .ToListAsync(cancellation);

        if (inventories.Count == 0)
            throw new ConflictException("Инвентарь пуст");

        var inventory = inventories.MinBy(ui => ui.Date)!;

        _logger.Log(NLog.LogLevel.Info, "SelledLast: UserId - {0}," +
                                       " GameItemId - {1}, Game - {2}",
                                       user.Id, inventory.ItemId,
                                       inventory.Item?.Game?.Name);

        context.UserInventories.Remove(inventory);
        await context.SaveChangesAsync(cancellation);

        await publisher.SendAsync(inventory.ToTemplate(), cancellation);

        return new SellItemResponse { Cost = inventory.FixedCost };
    }

    public async Task<UserInventoryResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var inventory = await context.UserInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(ui => ui.Id == id, cancellation) ??
            throw new NotFoundException("Предмет не найден в инвентаре");

        context.UserInventories.Remove(inventory);
        await context.SaveChangesAsync(cancellation);

        await publisher.SendAsync(new SiteStatisticsAdminTemplate { FundsUsersInventories = -inventory.FixedCost }, cancellation);

        return inventory.ToResponse();
    }
}