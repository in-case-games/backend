using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Game.Api.Controllers
{
    [Route("game/api/[controller]")]
    [ApiController]
    public class OpenCaseController : ControllerBase
    {
        private static readonly Random _random = new();
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public OpenCaseController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetOpeningCase(Guid caseId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            GameCase? gameCase = await context.GameCase
                .Include(x => x.СaseInventories)
                .FirstOrDefaultAsync(x => x.Id == caseId);

            if (userInfo is null || gameCase is null) return NotFound();
            if (userInfo.UserBalance < gameCase.GameCaseCost) return Forbid();

            //Update Balance Case and User
            userInfo.UserBalance -= gameCase.GameCaseCost;
            gameCase.GameCaseBalance += gameCase.GameCaseCost;

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in gameCase);

            //Update Balance Case
            decimal expensesCase = winGameItem.GameItemCost + (gameCase.GameCaseCost 
                * gameCase.RevenuePrecentage);
            gameCase.GameCaseBalance -= expensesCase;

            //Add history and add inventory user
            UserHistoryOpeningCases history = new()
            {
                Id = new Guid(),
                UserId = UserId,
                GameCaseId = gameCase.Id,
                GameItemId = winGameItem.Id,
                CaseOpenAt = DateTime.UtcNow
            };
            UserInventory inventory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                GameItemId = winGameItem.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(30),
            };

            await context.UserHistoryOpeningCases.AddAsync(history);
            await context.UserInventory.AddAsync(inventory);

            await context.SaveChangesAsync();

            return Ok(winGameItem);
        }
        // TODO: Rebase this
        #region nonAction
        private static GameItem RandomizeBySmallest(in GameCase gameCase)
        {
            List<int> lossChances = gameCase.СaseInventories!
                .Select(x => x.LossChance)
                .ToList();

            int winIndexItem = Randomizer(lossChances);
            GameItem winGameItem = gameCase.СaseInventories![winIndexItem].GameItem!;
            Guid winIdGameItem = winGameItem.Id;

            //Check it will become negative case balance
            if (IsProfitCase(winGameItem, gameCase) is false)
            {
                List<GameItem> gameItems = gameCase.СaseInventories.Select(x => x.GameItem).ToList()!;
                winGameItem = gameItems.MinBy(x => x.GameItemCost)!;
                winIdGameItem = winGameItem.Id;
            }

            return winGameItem;
        }

        private static bool IsProfitCase(GameItem gameItem, GameCase gameCase)
        {
            decimal RevenuePrecentage = gameCase.GameCaseBalance * gameCase.RevenuePrecentage;
            decimal AvailableBalance = gameCase.GameCaseBalance - RevenuePrecentage;

            return gameItem.GameItemCost <= AvailableBalance;
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
