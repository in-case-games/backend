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

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            LootBox? lootBox = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
            UserPathBanner? pathBanner = null;

            if (userInfo is null || lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));
            if (lootBox.IsLocked)
                return ResponseUtil.Conflict("Loot box is locked");
            if (userInfo.Balance < lootBox.Cost) 
                return ResponseUtil.Conflict("Insufficient funds");
            if (lootBox!.Banner?.Id is not null)
            {
                pathBanner = await context.UserPathBanners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.BannerId == lootBox.Banner!.Id && f.UserId == UserId);
            }

            //Update Balance Case and User
            userInfo.Balance -= lootBox.Cost;
            lootBox.Balance += lootBox.Cost;

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in lootBox);
            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync();

            decimal revenue = lootBox.Cost * RevenuePrecentage;
            decimal lootBoxExpenses = winGameItem.Cost + revenue;

            if (pathBanner is not null && lootBox.Banner!.IsActive == true)
            {
                --pathBanner.NumberSteps;

                decimal retentionAmount = lootBox.Cost * RetentionPrecentageBanner;
                decimal cashBack = GetCashBack(ref winGameItem, in lootBox, pathBanner);

                lootBoxExpenses = winGameItem.Cost + retentionAmount;

                if (pathBanner.NumberSteps == 0)
                {
                    winGameItem = lootBox.Inventories!
                        .FirstOrDefault(f => f.ItemId == pathBanner.ItemId)!.Item!;
                    winGameItem.Cost = pathBanner.FixedCost;

                    lootBoxExpenses = retentionAmount;
                }
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

            statisticsAdmin.BalanceWithdrawn += revenue;
            lootBox.Balance -= lootBoxExpenses;

            context.LootBoxes.Attach(lootBox);
            context.UserAdditionalInfos.Attach(userInfo);
            context.Entry(userInfo).Property(p => p.Balance).IsModified = true;
            context.Entry(lootBox).Property(p => p.Balance).IsModified = true;

            //Add history and add inventory user
            UserHistoryOpening history = new()
            {
                Id = new Guid(),
                UserId = UserId,
                BoxId = lootBox.Id,
                ItemId = winGameItem.Id,
                Date = DateTime.UtcNow
            };
            UserInventory inventory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                ItemId = winGameItem.Id,
                Date = DateTime.UtcNow,
                FixedCost = winGameItem.Cost
            };

            await context.UserHistoryOpenings.AddAsync(history);
            await context.UserInventories.AddAsync(inventory);

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winGameItem);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("virtual/{id}")]
        public async Task<IActionResult> GetVirtualOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);
            LootBox? lootBox = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (userInfo is null || lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));
            if (!userInfo.IsGuestMode)
                return Forbid("On guest mode");

            //Update Balance Case and User
            lootBox.VirtualBalance += lootBox.Cost;

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in lootBox);

            decimal revenue = lootBox.Cost * RevenuePrecentage;
            decimal expensesCase = winGameItem.Cost + revenue;

            lootBox.VirtualBalance -= expensesCase;

            context.LootBoxes.Attach(lootBox);
            context.Entry(lootBox).Property(p => p.VirtualBalance).IsModified = true;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winGameItem);
        }

        // TODO: Rebase this
        #region nonAction
        private static GameItem RandomizeBySmallest(in LootBox lootBox)
        {
            List<int> lossChances = lootBox.Inventories!
                .Select(x => x.ChanceWining)
                .ToList();

            int winIndexItem = Randomizer(lossChances);
            GameItem winGameItem = lootBox.Inventories![winIndexItem].Item!;
            Guid winIdGameItem = winGameItem.Id;

            //Check it will become negative case balance
            if (IsProfitCase(winGameItem, lootBox) is false)
            {
                List<GameItem> gameItems = lootBox.Inventories
                    .Select(x => x.Item)
                    .ToList()!;
                winGameItem = gameItems
                    .MinBy(x => x.Cost)!;
                winIdGameItem = winGameItem.Id;
            }

            return winGameItem;
        }
        private static decimal GetCashBack(
            ref GameItem winGameItem, 
            in LootBox lootBox, 
            UserPathBanner pathBanner)
        {
            decimal retentionAmount = lootBox.Cost * RetentionPrecentageBanner;
            decimal ceilingNumberSteps = Math.Ceiling(pathBanner.FixedCost / retentionAmount);
            decimal exactNumberSteps = pathBanner.FixedCost / retentionAmount;

            //Зачисление разницы между шагами округления 64.1 это 65 разница 0.9 * на процент шаг с кейса
            if (pathBanner.NumberSteps == 0)
                return (ceilingNumberSteps - exactNumberSteps) * retentionAmount;

            //Зачисление если предмет выпал раньше того как пользователь дойдет до предмета
            if (pathBanner.ItemId == winGameItem.Id)
                return (ceilingNumberSteps - pathBanner.NumberSteps) * retentionAmount;

            return -1M;
        }
        private static bool IsProfitCase(GameItem gameItem, LootBox lootBox)
        {
            decimal revenue = lootBox.Balance * RevenuePrecentage;
            decimal availableBalance = lootBox.Balance - revenue;

            return gameItem.Cost <= availableBalance;
        }

        private static int Randomizer(List<int> lossChance)
        {
            List<List<int>> partsCaseChance = new();
            int startParts = 0;
            int lengthPart;
            int maxRandomValue;
            int randomNumber;
            int winIndex = 0;

            for (int i = 0; i < lossChance.Count; i++)
            {
                lengthPart = lossChance[i];
                partsCaseChance.Add(new List<int>() { startParts, startParts + lengthPart - 1 });
                startParts += lengthPart;
            }
            maxRandomValue = partsCaseChance[^1][1];
            randomNumber = _random.Next(0, maxRandomValue + 1);

            for (int i = 0; i < partsCaseChance.Count; i++)
            {
                List<int> part = partsCaseChance[i];
                if (part[0] <= randomNumber && part[1] >= randomNumber)
                {
                    winIndex = i;
                }
            }

            return winIndex;
        }
        #endregion
    }
}
