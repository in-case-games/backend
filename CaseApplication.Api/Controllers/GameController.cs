using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly Random _random = new();
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GameController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetOpeningCase(Guid caseId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userAdditionalInfo = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            GameCase? gameCase = await context.GameCase
                .FirstOrDefaultAsync(x => x.Id == caseId);

            if (userAdditionalInfo is null || gameCase is null) return NotFound();

            bool isValidBalance = (userAdditionalInfo.UserBalance >= gameCase.GameCaseCost);

            if (isValidBalance is false) {
                return Forbid("Your balance is less than the cost of the case" +
                    "Top up your balance or open a case cheaper");
            }

            gameCase.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                .Include(x => x.GameItem)
                .Where(x => x.GameCaseId == gameCase.Id)
                .ToListAsync();

            //Update Balance Case and User
            userAdditionalInfo.UserBalance -= gameCase.GameCaseCost;
            gameCase.GameCaseBalance += gameCase.GameCaseCost;

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in gameCase);

            //Update Balance Case
            decimal expensesCase = winGameItem.GameItemCost + (gameCase.GameCaseCost 
                * gameCase.RevenuePrecentage);
            gameCase.GameCaseBalance -= expensesCase;

            //Add history and add inventory user
            UserHistoryOpeningCases userHistory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                GameCaseId = gameCase.Id,
                GameItemId = winGameItem.Id,
                CaseOpenAt = DateTime.UtcNow
            };
            UserInventory userInventory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                GameItemId = winGameItem.Id
            };

            await context.UserHistoryOpeningCases.AddAsync(userHistory);
            await context.UserInventory.AddAsync(userInventory);

            await context.SaveChangesAsync();

            return Ok(winGameItem);
        }
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

                gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();
                winGameItem = gameItems[^1];
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
