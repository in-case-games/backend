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
            LootBox box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            UserAdditionalInfo info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            UserPromocode? promocode = await _context.UserPromocodes
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

            UserPathBanner? path = await _context.PathBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BoxId == box.Id && upb.UserId == userId);

            decimal discount = promocode?.Discount ?? 0;
            decimal boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

            info.Balance -= boxCost;
            box.Balance += boxCost;

            GameItem winItem = OpenLootBoxService.RandomizeBySmallest(in box);

            decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
            decimal expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

            if (path is not null && 
                box.ExpirationBannerDate is not null && 
                box.ExpirationBannerDate >= DateTime.UtcNow)
            {
                --path!.NumberSteps;

                decimal retentionAmount = OpenLootBoxService.GetRetentionAmount(box.Cost);
                decimal cashBack = OpenLootBoxService.GetCashBack(winItem.Id, box.Cost, path);

                OpenLootBoxService.CheckWinItemAndExpenses(ref winItem, ref expenses, box, path);
                OpenLootBoxService.CheckCashBackAndRevenue(
                    ref revenue, ref path, ref info,
                    cashBack, _context);
            }

            SiteStatisticsTemplate statisticsTemplate = new() { LootBoxes = 1 };
            SiteStatisticsAdminTemplate statisticsAdminTemplate = new() { BalanceWithdrawn = revenue };

            await _publisher.SendAsync(statisticsTemplate);
            await _publisher.SendAsync(statisticsAdminTemplate);

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
            UserAdditionalInfo userInfo = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            LootBox box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            if (!userInfo.IsGuestMode)
                throw new ForbiddenException("Не включен режим гостя");

            box.VirtualBalance += box.Cost;

            GameItem winItem = OpenLootBoxService.RandomizeBySmallest(in box, true);
            decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
            decimal expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

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

            UserAdditionalInfo userInfo = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            LootBox box = await _context.Boxes
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

            for (int i = 0; i < count; i++)
            {
                try
                {
                    box.VirtualBalance += box.Cost;

                    GameItem item = OpenLootBoxService.RandomizeBySmallest(in box, true);
                    decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
                    decimal expenses = OpenLootBoxService.GetExpenses(item.Cost, revenue);

                    box.VirtualBalance -= expenses;

                    int index = winItems.FindIndex(gi => gi.Id == item.Id);

                    if (index != -1)
                        winItems[index].Count++;
                    else
                        winItems.Add(new() { Id = item.Id, Cost = item.Cost, Count = 1 });
                }
                catch(Exception ex)
                {
                    await _context.SaveChangesAsync();

                    if (count == 0) throw new Exception(ex.Message);

                    throw new StatusCodeExtendedException(ErrorCodes.UnknownError, ex.Message, winItems);
                }
            }

            await _context.SaveChangesAsync();

            return winItems;
        }
    }
}
