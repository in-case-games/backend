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
        private const decimal RevenuePrecentage = 0.1M;
        private const decimal RevenuePrecentageBanner = 0.04M;
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
        public async Task<IActionResult> GetOpeningCase(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            LootBox? lootBox = await context.LootBoxes
                .Include(i => i.Inventories)
                .Include(i => i.Banner)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (userInfo is null || lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));
            if (userInfo.Balance < lootBox.Cost) 
                return Forbid("Insufficient funds");

            UserPathBanner? pathBanner = await context.UserPathBanners
                .Include(i => i.Item)
                .FirstOrDefaultAsync(f => f.BannerId == lootBox.Banner!.Id && f.UserId == UserId);

            //Update Balance Case and User
            userInfo.Balance -= lootBox.Cost;
            lootBox.Balance += lootBox.Cost;

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in lootBox);

            decimal expensesCase = winGameItem.Cost + lootBox.Cost * RevenuePrecentage;

            if (pathBanner is not null)
            {
                pathBanner.NumberSteps--;
                expensesCase = winGameItem.Cost + lootBox.Cost * 
                    (RevenuePrecentage - RevenuePrecentageBanner);

                if (pathBanner.NumberSteps <= 0 && pathBanner.ItemId == winGameItem.Id)
                {
                    winGameItem = pathBanner.Item!;
                    context.UserPathBanners.Remove(pathBanner);
                }
            }

            //Update Balance Case
            lootBox.Balance -= expensesCase;

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

            return Ok(new { Data = winGameItem, Success = true });
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
