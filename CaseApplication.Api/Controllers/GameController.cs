using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly Random random = new();

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

            CheckErrorBalance(userAdditionalInfo, gameCase);

            //Update Balance Case and User
            userAdditionalInfo = await UpdateUserBalance(userAdditionalInfo, -gameCase.GameCaseCost);
            gameCase = await UpdateCaseBalance(gameCase, gameCase.GameCaseCost);

            //Get All the chances of items from the case 
            List<CaseInventory> casesInventories = (await _caseInventoryRepository
                .GetAll(caseId))
                .ToList();

            List<int> lossChances = casesInventories
                .Where(x => true)
                .Select(x => x.LossChance)
                .ToList();

            //Calling random
            //GameItem winGameItem = await RandomizeBySmallest(lossChances, casesInventories, gameCase);
            GameItem winGameItem = await RandomizeByLadder(lossChances, casesInventories, gameCase);

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
        private async Task<GameItem> RandomizeBySmallest(
            List<int> lossChances,
            List<CaseInventory> casesInventories,
            GameCase gameCase)
        {
            int winIndexItem = Randomizer(lossChances);
            Guid winIdGameItem = casesInventories[winIndexItem].GameItemId;
            GameItem winGameItem = await _gameItemRepository.Get(winIdGameItem);

            //Check it will become negative case balance
            if (IsProfitCase(winGameItem, gameCase) is false)
            {
                List<GameItem> gameItems = new();
                GameItem searchItem;

                foreach (CaseInventory invetory in casesInventories)
                {
                    searchItem = await _gameItemRepository.Get(invetory.GameItemId);
                    gameItems.Add(searchItem);
                }

                gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();
                winIdGameItem = gameItems[^1].Id;
                winGameItem = gameItems[^1];
                /*foreach(GameItem gameItem in gameItems)
                {
                    if(IsProfitCase(gameItem, gameCase) && 
                        gameItem.GameItemCost <= winGameItem.GameItemCost)
                    {
                        winIdGameItem = gameItem.Id;
                        winGameItem = gameItem;
                    }
                }*/
            }

            return winGameItem;
        }

        private async Task<GameItem> RandomizeByLadder(
            List<int> lossChances,
            List<CaseInventory> casesInventories,
            GameCase gameCase)
        {
            int winIndexItem = Randomizer(lossChances);
            Guid winIdGameItem = casesInventories[winIndexItem].GameItemId;
            GameItem winGameItem = await _gameItemRepository.Get(winIdGameItem);

            //Check it will become negative case balance
            if (IsProfitCase(winGameItem, gameCase) is false)
            {
                List<GameItem> gameItems = new();
                GameItem searchItem;

                foreach (CaseInventory invetory in casesInventories)
                {
                    searchItem = await _gameItemRepository.Get(invetory.GameItemId);
                    gameItems.Add(searchItem);
                }

                gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();

                foreach(GameItem gameItem in gameItems)
                {
                    if(IsProfitCase(gameItem, gameCase) && 
                        gameItem.GameItemCost <= winGameItem.GameItemCost)
                    {
                        winIdGameItem = gameItem.Id;
                        winGameItem = gameItem;
                    }
                }
            }

            return winGameItem;
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
        private static void CheckErrorBalance(UserAdditionalInfo info, GameCase gameCase)
        {
            bool IsValidBalance = (info.UserBalance >= gameCase.GameCaseCost);

            if (IsValidBalance is false)
                throw new Exception("Your balance is less than the cost of the case" +
                    "Top up your balance or open a case cheaper");
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
            randomNumber = random.Next(0, maxRandomValue + 1);

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
