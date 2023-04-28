using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
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

            LootBox? box = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            UserPathBanner? pathBanner = null;

            if (box is null)
                return ResponseUtil.NotFound(nameof(LootBox));

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.HistoryPromocodes!)
                    .ThenInclude(ti => ti.Promocode)
                        .ThenInclude(ti => ti!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == UserId);

            if(user is null || user.AdditionalInfo is null)
                return ResponseUtil.NotFound("User");

            UserAdditionalInfo userInfo = user.AdditionalInfo;
            UserHistoryPromocode? promocode = user.HistoryPromocodes?
                .FirstOrDefault(f => f.IsActivated == false && f.Promocode?.Type?.Name == "case");

            if (box.IsLocked)
                return ResponseUtil.Conflict("Loot box is locked");
            if (userInfo.Balance < box.Cost) 
                return ResponseUtil.Conflict("Insufficient funds");
            if (box.Banner?.Id is not null)
            {
                pathBanner = await context.UserPathBanners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.BannerId == box.Banner!.Id && f.UserId == UserId);
            }

            decimal discount = 0;

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

            return ResponseUtil.Ok(winItem);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("virtual/{id}")]
        public async Task<IActionResult> GetVirtualOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);
            LootBox? box = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (userInfo is null || box is null)
                return ResponseUtil.NotFound(nameof(LootBox));
            if (!userInfo.IsGuestMode)
                return Forbid();

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

            return ResponseUtil.Ok(winItem);
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
