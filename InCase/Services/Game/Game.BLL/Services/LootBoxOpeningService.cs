using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class LootBoxOpeningService : ILootBoxOpeningService
    {
        private readonly ApplicationDbContext _context;

        public LootBoxOpeningService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameItemResponse> OpenBox(Guid userId, Guid id)
        {
            LootBox box = await _context.Boxes
                .Include(lb => lb.Inventories!)
                .ThenInclude(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            UserAdditionalInfo userInfo = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == userId) ??
                throw new NotFoundException("Пользователь не найден");

            UserPromocode? promocode = await _context.HistoryPromocodes
                .FirstOrDefaultAsync(uhp => uhp.UserId == userId);

            if (box.IsLocked)
                throw new ForbiddenException("Кейс заблокирован");
            if (userInfo.Balance < box.Cost)
                throw new PaymentRequiredException("Недостаточно средств");

            UserPathBanner? pathBanner = await _context.PathBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BoxId == box.Id && upb.UserId == userId);

            decimal discount = promocode?.Discount ?? 0;
            decimal boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

            userInfo.Balance -= boxCost;
            box.Balance += boxCost;

            GameItem winItem = OpenLootBoxService.RandomizeBySmallest(in box);

            decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
            decimal expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

            if (pathBanner is not null && box.IsActiveBanner)
            {
                --pathBanner!.NumberSteps;

                decimal retentionAmount = OpenLootBoxService.GetRetentionAmount(box.Cost);
                decimal cashBack = OpenLootBoxService.GetCashBack(winItem.Id, box.Cost, pathBanner);

                OpenLootBoxService.CheckWinItemAndExpenses(ref winItem, ref expenses, box, pathBanner);
                OpenLootBoxService.CheckCashBackAndRevenue(
                    ref revenue, ref pathBanner, ref userInfo,
                    cashBack, _context);
            }

            //Notify by rabbit mq statistics
            box.Balance -= expenses;

            _context.Boxes.Attach(box);
            _context.AdditionalInfos.Attach(userInfo);
            _context.Entry(userInfo).Property(p => p.Balance).IsModified = true;
            _context.Entry(box).Property(p => p.Balance).IsModified = true;

            DateTime date = DateTime.UtcNow;

            UserOpening opening = new()
            {
                Id = new Guid(),
                UserId = userId,
                BoxId = box.Id,
                ItemId = winItem.Id,
                Date = date
            };

            //Notify by rabbit mq user inventory add new item

            await _context.HistoryOpenings.AddAsync(opening);

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
    }
}
