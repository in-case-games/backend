using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class LootBoxOpeningService : ILootBoxOpeningService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxOpeningService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<GameItemResponse> OpenBox(Guid userId, Guid id)
        {
            var box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            var info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            var promocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.UserId == userId);

            if (box.IsLocked)
                throw new ForbiddenException("Кейс заблокирован");
            if (info.Balance < box.Cost)
                throw new PaymentRequiredException("Недостаточно средств");
            if (promocode is not null)
            {
                UserPromocodeBackTemplate templatePromo = promocode.ToTemplate();

                await _publisher.SendAsync(templatePromo);

                _context.UserPromocodes.Remove(promocode);
            }

            var path = await _context.PathBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BoxId == box.Id && upb.UserId == userId);

            var discount = promocode?.Discount ?? 0;
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

                var cashBack = OpenLootBoxService.GetCashBack(winItem.Id, box.Cost, path);

                OpenLootBoxService.CheckWinItemAndExpenses(ref winItem, ref expenses, box, path);
                OpenLootBoxService.CheckCashBackAndRevenue(
                    ref revenue, ref path, ref info,
                    cashBack, _context);
            }

            await _publisher.SendAsync(new SiteStatisticsTemplate { LootBoxes = 1 });
            await _publisher.SendAsync(new SiteStatisticsAdminTemplate { FundsUsersInventories = revenue });

            box.Balance -= expenses;

            _context.Entry(info).Property(p => p.Balance).IsModified = true;
            _context.Entry(box).Property(p => p.Balance).IsModified = true;

            UserOpening opening = new()
            {
                UserId = userId,
                BoxId = box.Id,
                ItemId = winItem.Id,
                Date = DateTime.UtcNow
            };

            UserInventoryTemplate templateUser = new()
            {
                Date = DateTime.UtcNow,
                FixedCost = winItem.Cost,
                ItemId = winItem.Id,
                UserId = userId,
            };

            await _publisher.SendAsync(templateUser);
            await _context.Openings.AddAsync(opening);
            await _context.SaveChangesAsync();

            return winItem.ToResponse();
        }

        public async Task<GameItemResponse> OpenVirtualBox(Guid userId, Guid id)
        {
            var userInfo = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            var box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            if (!userInfo.IsGuestMode)
                throw new ForbiddenException("Не включен режим гостя");

            box.VirtualBalance += box.Cost;

            var winItem = OpenLootBoxService.RandomizeBySmallest(in box, true);
            var revenue = OpenLootBoxService.GetRevenue(box.Cost);
            var expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

            box.VirtualBalance -= expenses;

            _context.Boxes.Attach(box);
            _context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;

            await _context.SaveChangesAsync();

            return winItem.ToResponse();
        }


        public async Task<List<GameItemBigOpenResponse>> OpenVirtualBox(
            Guid userId,
            Guid id, 
            int count, 
            bool isAdmin = false)
        {
            if (count < 1 || (count > 100 && !isAdmin))
                throw new BadRequestException("Количество открытий должно быть в диапазоне от 1 до 100");

            var userInfo = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            var box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            if (!userInfo.IsGuestMode)
                throw new ForbiddenException("Не включен режим гостя");

            _context.Boxes.Attach(box);
            _context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;

            List<GameItemBigOpenResponse> winItems = new();

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

                    if (index != -1)
                        winItems[index].Count++;
                    else
                        winItems.Add(new GameItemBigOpenResponse { Id = item.Id, Cost = item.Cost, Count = 1 });
                }
                catch(Exception ex)
                {
                    await _context.SaveChangesAsync();

                    throw new StatusCodeExtendedException(ErrorCodes.UnknownError, ex.Message, winItems);
                }
            }

            await _context.SaveChangesAsync();

            return winItems;
        }
    }
}
