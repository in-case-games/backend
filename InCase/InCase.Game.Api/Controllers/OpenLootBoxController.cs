using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Game.Api.Controllers
{
    [Route("api/open-loot-box")]
    [ApiController]
    public class OpenLootBoxController : ControllerBase
    {
        private const decimal RevenuePrecentage = 0.10M;
        private const decimal RetentionPrecentageBanner = 0.20M;
        private static readonly Random _random = new();
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public OpenLootBoxController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox box = await context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi!.Item)
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundCodeException("Кейс не найден");
            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Пользователь не найден");

            UserHistoryPromocode? promocode = await context.UserHistoryPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .FirstOrDefaultAsync(uhp => 
                uhp.UserId == UserId &&
                uhp.IsActivated == false && 
                uhp.Promocode!.Type!.Name == "case");

            UserPathBanner? pathBanner = null;
            decimal discount = 0;

            if (box.IsLocked)
                throw new ForbiddenCodeException("Кейс заблокирован");
            if (userInfo.Balance < box.Cost)
                throw new PaymentRequiredCodeException("Недостаточно средств");

            if (box.Banner?.Id is not null)
            {
                pathBanner = await context.UserPathBanners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(upb => upb.BannerId == box.Banner!.Id && upb.UserId == UserId);
            }
            if (promocode is not null)
            {
                discount = promocode.Promocode!.Discount;

                promocode.IsActivated = true;
                promocode.Date = DateTime.UtcNow;

                context.UserHistoryPromocodes.Attach(promocode);
                context.Entry(promocode).Property(p => p.IsActivated).IsModified = true;
                context.Entry(promocode).Property(p => p.Date).IsModified = true;
            }

            decimal boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

            //Update Balance Case and User
            userInfo.Balance -= boxCost;
            box.Balance += boxCost;

            //Calling random
            GameItem winItem = RandomizeBySmallest(in box);
            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync();
            SiteStatistics statistics = await context.SiteStatistics
                .FirstAsync();

            decimal revenue = box.Cost * RevenuePrecentage;
            decimal expenses = winItem.Cost + revenue;

            if (pathBanner is not null && box.Banner!.IsActive == true)
            {
                --pathBanner.NumberSteps;

                decimal retentionAmount = box.Cost * RetentionPrecentageBanner;
                decimal cashBack = GetCashBack(winItem.Id, box.Cost, pathBanner);

                CheckWinItemAndExpenses(ref winItem, ref expenses, box, pathBanner);

                if (cashBack >= 0)
                {
                    userInfo.Balance += cashBack * (1M - RevenuePrecentage);
                    revenue = cashBack * RevenuePrecentage;

                    context.UserPathBanners.Remove(pathBanner);
                }
                else
                {
                    revenue = 0;
                    context.UserPathBanners.Attach(pathBanner);
                    context.Entry(pathBanner).Property(p => p.NumberSteps).IsModified = true;
                }
            }

            statistics.LootBoxes++;

            statisticsAdmin.BalanceWithdrawn += revenue;
            box.Balance -= expenses;

            if (box.Balance < 0)
            {
                //Notify admin negative balance
                statisticsAdmin.BalanceWithdrawn += box.Balance;
                box.Balance = 0;
            }

            context.LootBoxes.Attach(box);
            context.UserAdditionalInfos.Attach(userInfo);
            context.Entry(userInfo).Property(p => p.Balance).IsModified = true;
            context.Entry(box).Property(p => p.Balance).IsModified = true;

            DateTime date = DateTime.UtcNow; 

            //Add history and add inventory user
            UserHistoryOpening history = new()
            {
                Id = new Guid(),
                UserId = UserId,
                BoxId = box.Id,
                ItemId = winItem.Id,
                Date = date
            };
            UserInventory inventory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                ItemId = winItem.Id,
                Date = date,
                FixedCost = winItem.Cost
            };

            await context.UserHistoryOpenings.AddAsync(history);
            await context.UserInventories.AddAsync(inventory);

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winItem.Convert(false));
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("virtual/{id}")]
        public async Task<IActionResult> GetVirtualOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Пользователь не найден");
            LootBox? box = await context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi!.Item)
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundCodeException("Кейс не найден");

            if (!userInfo.IsGuestMode)
                throw new ForbiddenCodeException("Не включен режим гостя");

            //Update Balance Case and User
            box.VirtualBalance += box.Cost;

            //Calling random
            GameItem winItem = RandomizeBySmallest(in box, true);

            decimal revenue = box.Cost * RevenuePrecentage;
            decimal expensesCase = winItem.Cost + revenue;

            box.VirtualBalance -= expensesCase;

            context.LootBoxes.Attach(box);
            context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winItem.Convert(false));
        }

        #region nonAction
        private static GameItem RandomizeBySmallest(in LootBox box, bool IsVirtual = false)
        {
            List<int> chances = box.Inventories!
                .Select(s => s.ChanceWining)
                .ToList();

            int index = Randomizer(chances);
            GameItem item = box.Inventories![index].Item!;

            if (IsProfitCase(item, box, IsVirtual) is false)
            {
                List<GameItem> items = box.Inventories
                    .Select(s => s.Item)
                    .ToList()!;

                item = items.MinBy(m => m.Cost)!;
            }

            return item;
        }
        private static decimal GetCashBack(
            Guid itemGuid, 
            decimal boxCost, 
            UserPathBanner pathBanner)
        {
            decimal retention = boxCost * RetentionPrecentageBanner;
            decimal exact = pathBanner.FixedCost / retention;
            decimal ceiling = Math.Ceiling(exact);

            if (pathBanner.NumberSteps == 0)
                return (ceiling - exact) * retention;
            else if (pathBanner.ItemId == itemGuid)
                return (ceiling - pathBanner.NumberSteps) * retention;
            else
                return -1M;
        }

        private static void CheckWinItemAndExpenses(
            ref GameItem winItem, 
            ref decimal expenses,
            in LootBox box,
            UserPathBanner pathBanner)
        {
            decimal retention = box.Cost * RetentionPrecentageBanner;
            expenses = winItem.Cost + retention;

            if (pathBanner.NumberSteps == 0)
            {
                winItem = box.Inventories!
                    .FirstOrDefault(f => f.ItemId == pathBanner.ItemId)!.Item!;
                winItem.Cost = pathBanner.FixedCost;

                expenses = retention;
            }
        }

        private static bool IsProfitCase(GameItem item, LootBox box, bool IsVirtual = false)
        {
            decimal balance = IsVirtual ? box.VirtualBalance : box.Balance;
            decimal revenue = balance * RevenuePrecentage;
            decimal availableBalance = balance - revenue;

            return item.Cost <= availableBalance;
        }

        private static int Randomizer(List<int> chances)
        {
            List<List<int>> partsChances = new();
            int start = 0;
            int length;
            int index = 0;

            for (int i = 0; i < chances.Count; i++)
            {
                length = chances[i];
                partsChances.Add(new List<int>() { start, start + length - 1 });
                start += length;
            }

            int maxValue = partsChances[^1][1];
            int random = _random.Next(0, maxValue + 1);

            for (int i = 0; i < partsChances.Count; i++)
            {
                List<int> part = partsChances[i];
                if (part[0] <= random && part[1] >= random)
                {
                    index = i;
                }
            }

            return index;
        }
        #endregion
    }
}
