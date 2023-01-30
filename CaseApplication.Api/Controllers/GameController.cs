using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly Random _random = new();

        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private readonly IGameCaseRepository _gameCaseRepository;
        private readonly ICaseInventoryRepository _caseInventoryRepository;
        private readonly IUserHistoryOpeningCasesRepository _userHistory;
        private readonly IGameItemRepository _gameItemRepository;
        private readonly IUserInventoryRepository _userInventoryRepository;

        public GameController(
            IUserAdditionalInfoRepository userInfoRepository,
            ICaseInventoryRepository caseInventoryRepository,
            IUserHistoryOpeningCasesRepository userHistory,
            IGameCaseRepository gameCaseRepository,
            IGameItemRepository gameItemRepository,
            IUserInventoryRepository userInventoryRepository)
        {
            _userInfoRepository = userInfoRepository;
            _caseInventoryRepository = caseInventoryRepository;
            _userHistory = userHistory;
            _gameCaseRepository = gameCaseRepository;
            _gameItemRepository = gameItemRepository;
            _userInventoryRepository = userInventoryRepository;
        }

        [HttpGet]
        public async Task<GameItem> GetOpeningCase(Guid userId, Guid caseId)
        {
            //Check Balance
            UserAdditionalInfo userAdditionalInfo = await _userInfoRepository.Get(userId);
            GameCase gameCase = await _gameCaseRepository.Get(caseId);

            bool isValidBalance = (userAdditionalInfo.UserBalance >= gameCase.GameCaseCost);

            if (isValidBalance is false)
                throw new Exception("Your balance is less than the cost of the case" +
                    "Top up your balance or open a case cheaper");

            //Update Balance Case and User
            await UpdateUserBalance(userAdditionalInfo, -gameCase.GameCaseCost);
            gameCase = await UpdateCaseBalance(gameCase, gameCase.GameCaseCost);

            //Calling random
            GameItem winGameItem = await RandomizeByConstraints(caseId, gameCase.GameCaseBalance);

            //Update User and Case Balance
            decimal expensesCase = winGameItem.GameItemCost + (gameCase.GameCaseCost 
                * gameCase.RevenuePrecentage);
            gameCase = await UpdateCaseBalance(gameCase, -expensesCase);

            //Add history and add inventory user
            UserHistoryOpeningCases historyCase = new()
            {
                UserId = userId,
                GameCaseId = gameCase.Id,
                GameItemId = winGameItem.Id,
                CaseOpenAt = DateTime.UtcNow
            };
            UserInventory userInventory = new()
            {
                UserId = userId,
                GameItemId = winGameItem.Id
            };

            await _userHistory.Create(historyCase);
            await _userInventoryRepository.Create(userInventory);

            return winGameItem;
        }

        private async Task<GameItem> RandomizeByConstraints(Guid caseId, decimal balance)
        {
            List<CaseInventory> casesInventories = (await _caseInventoryRepository
                .GetAll(caseId))
                .ToList();

            IEnumerable<GameItem> gameItems = await _gameItemRepository.GetAll();

            gameItems = (from i in gameItems
                         from j in casesInventories
                         orderby i.GameItemCost descending
                         where i.GameItemCost < balance
                         && i.Id == j.GameItemId
                         select i);

            casesInventories = (from i in casesInventories
                               from j in gameItems
                               where i.GameItemId == j.Id
                               select i).ToList();

            List<int> lossChances = casesInventories
                .Select(x => x.LossChance)
                .ToList();

            return gameItems.ElementAt(Randomizer(lossChances));
        }
        private async Task<UserAdditionalInfo> UpdateUserBalance(UserAdditionalInfo info, decimal value)
        {
            info.UserBalance += value;
            await _userInfoRepository.Update(info);

            return info;
        }
        private async Task<GameCase> UpdateCaseBalance(GameCase gameCase, decimal value)
        {
            gameCase.GameCaseBalance += value;
            await _gameCaseRepository.Update(gameCase);

            return gameCase;
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
    }
}
