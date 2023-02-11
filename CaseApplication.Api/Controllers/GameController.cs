﻿using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly Random _random = new();
        #region injections
        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private readonly IGameCaseRepository _gameCaseRepository;
        private readonly IUserHistoryOpeningCasesRepository _userHistory;
        private readonly IUserInventoryRepository _userInventoryRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        #endregion
        #region ctor
        public GameController(
            IUserAdditionalInfoRepository userInfoRepository,
            IUserHistoryOpeningCasesRepository userHistory,
            IGameCaseRepository gameCaseRepository,
            IUserInventoryRepository userInventoryRepository)
        {
            _userInfoRepository = userInfoRepository;
            _userHistory = userHistory;
            _gameCaseRepository = gameCaseRepository;
            _userInventoryRepository = userInventoryRepository;
        }
        #endregion
        [Authorize]
        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetOpeningCase(Guid caseId)
        {
            //Check Balance
            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.GetByUserId(UserId);
            GameCase? gameCase = await _gameCaseRepository.Get(caseId);

            if(userAdditionalInfo is null || gameCase is null) return NotFound();

            bool isValidBalance = (userAdditionalInfo.UserBalance >= gameCase.GameCaseCost);

            if (isValidBalance is false) {
                return Forbid("Your balance is less than the cost of the case" +
                    "Top up your balance or open a case cheaper");
            }

            //Update Balance Case and User
            await UpdateUserBalance(userAdditionalInfo, -gameCase.GameCaseCost);
            gameCase = await UpdateCaseBalance(gameCase, gameCase.GameCaseCost);

            //Calling random
            GameItem winGameItem = RandomizeBySmallest(in gameCase);

            //Update Balance Case
            decimal expensesCase = winGameItem.GameItemCost + (gameCase.GameCaseCost 
                * gameCase.RevenuePrecentage);
            gameCase = await UpdateCaseBalance(gameCase, -expensesCase);

            //Add history and add inventory user
            UserHistoryOpeningCasesDto historyCase = new()
            {
                UserId = UserId,
                GameCaseId = gameCase.Id,
                GameItemId = winGameItem.Id,
                CaseOpenAt = DateTime.UtcNow
            };
            UserInventoryDto userInventory = new()
            {
                UserId = UserId,
                GameItemId = winGameItem.Id
            };

            await _userHistory.Create(historyCase);
            await _userInventoryRepository.Create(userInventory);

            return Ok(winGameItem);
        }
        #region nonAction
        private static GameItem RandomizeBySmallest(in GameCase gameCase)
        {
            List<int> lossChances = gameCase.СaseInventories!
                .Select(x => x.LossChance)
                .ToList();

            int winIndexItem = Randomizer(lossChances);
            Guid winIdGameItem = gameCase.СaseInventories![winIndexItem].GameItemId;
            GameItem winGameItem = gameCase.СaseInventories[winIndexItem].GameItem!;

            //Check it will become negative case balance
            if (IsProfitCase(winGameItem, gameCase) is false)
            {
                List<GameItem> gameItems = gameCase.СaseInventories.Select(x => x.GameItem).ToList()!;

                gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();
                winIdGameItem = gameItems[^1].Id;
                winGameItem = gameItems[^1];
            }

            return winGameItem;
        }

        private static bool IsProfitCase(GameItem gameItem, GameCase gameCase)
        {
            decimal RevenuePrecentage = gameCase.GameCaseBalance * gameCase.RevenuePrecentage;
            decimal AvailableBalance = gameCase.GameCaseBalance - RevenuePrecentage;

            return gameItem.GameItemCost <= AvailableBalance;
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
        #endregion
    }
}
