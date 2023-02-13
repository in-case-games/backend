using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Xunit.Abstractions;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class GameApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private readonly ITestOutputHelper _output;
        public GameApiTest(
            WebApplicationFactory<Program> app,
            ITestOutputHelper output)
        {
            _response = new(app.CreateClient());
            _output = output;
        }

        [Fact]
        public async Task GET_OpeningCase_Output()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            Guid caseGuid = Guid.NewGuid();
            List<Guid> gameItemsGuids = new() { 
                Guid.NewGuid(), Guid.NewGuid(), 
                Guid.NewGuid(), Guid.NewGuid(), 
                Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(),
            };
            await InitializeTestUser(guid);
            await InitializeTestDependencies(gameItemsGuids, caseGuid);

            //Act
            Stopwatch startTime = Stopwatch.StartNew();
            Dictionary<string, int> winingItems = await GetWiningItems(caseGuid);
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:000}",
                resultTime.Seconds,
                resultTime.Milliseconds);

            //Assert
            _output.WriteLine("Предмет = количество выпадений");
            foreach(var winItem in winingItems)
            {
                _output.WriteLine($"{winItem.Key} = {winItem.Value}");
            }

            GameCase? gameCase = await _response.ResponseGet<GameCase?>(
                $"/GameCase/admin/{caseGuid}",
                AccessToken);

            _output.WriteLine(
                $"Профит: {gameCase!.GameCaseCost * gameCase.RevenuePrecentage * 1000} Р\n" +
                $"Баланс: {gameCase.GameCaseBalance} Р\n" +
                $"Скорость алгоритма: {elapsedTime}");

            await RemoveTestDependencies(gameItemsGuids, caseGuid);
            await RemoveTestUser(guid);
        }

        #region Начальные данные
        private async Task<Dictionary<string, int>> GetWiningItems(Guid gameCaseGuid)
        {
            Dictionary<string, int> winItems = new() {
                ["USP-S | Cortex (Немного поношенное)"] = 0,
                ["AK-47 | Ice Coaled (Закалённое в боях)"] = 0,
                ["Перчатки «Гидра» | Изумруд (Закаленное в боях)"] = 0,
                ["Обмотки рук | Пустынный шемах (Закаленное в боях)"] = 0,
                ["Фальшион | Ночь (После полевых испытаний)"] = 0,
                ["Glock-18 | Ласка (Немного поношенное)"] = 0,
                ["AWP | Ахерон (После полевых испытаний)"] = 0,
            };

            for(int i = 0; i <= 1000; i++)
            {
                //Open Cases
                GameItem? winItem = await _response
                    .ResponseGet<GameItem?>($"/Game/{gameCaseGuid}", AccessToken);
                if (winItem == null) throw new Exception("No opened cases");

                //Counter wins
                winItems[winItem.GameItemName!] += 1;
            }

            return winItems;
        }

        private async Task InitializeTestDependencies(List<Guid> gameItemsGuids, Guid gameCaseGuid)
        {
            //Create Game Item
            GameItem item1 = new()
            {
                Id = gameItemsGuids[0],
                GameItemName = "USP-S | Cortex (Немного поношенное)",
                GameItemCost = 239.99M,
                GameItemImage = "GOCSATImage1",
                GameItemRarity = "Засекреченное"
            };
            GameItem item2 = new()
            {
                Id = gameItemsGuids[1],
                GameItemName = "AK-47 | Ice Coaled (Закалённое в боях)",
                GameItemCost = 752M,
                GameItemImage = "GOCSATImage2",
                GameItemRarity = "Засекреченное"
            };
            GameItem item3 = new()
            {
                Id = gameItemsGuids[2],
                GameItemName = "Перчатки «Гидра» | Изумруд (Закаленное в боях)",
                GameItemCost = 4211M,
                GameItemImage = "GOCSATImage3",
                GameItemRarity = "Экстраординарного типа"
            };
            GameItem item4 = new()
            {
                Id = gameItemsGuids[3],
                GameItemName = "Обмотки рук | Пустынный шемах (Закаленное в боях)",
                GameItemCost = 4857M,
                GameItemImage = "GOCSATImage4",
                GameItemRarity = "Экстраординарного типа"
            };
            GameItem item5 = new()
            {
                Id = gameItemsGuids[4],
                GameItemName = "Фальшион | Ночь (После полевых испытаний)",
                GameItemCost = 8000M,
                GameItemImage = "GOCSATImage5",
                GameItemRarity = "Тайное"
            };
            GameItem item6 = new()
            {
                Id = gameItemsGuids[5],
                GameItemName = "Glock-18 | Ласка (Немного поношенное)",
                GameItemCost = 66M,
                GameItemImage = "GOCSATImage6",
                GameItemRarity = "Армейское качество"
            };
            GameItem item7 = new()
            {
                Id = gameItemsGuids[6],
                GameItemName = "AWP | Ахерон (После полевых испытаний)",
                GameItemCost = 61M,
                GameItemImage = "GOCSATImage7",
                GameItemRarity = "Армейское качество"
            };
            List<GameItem> gameItems = new() {
                item1, item2, item3, item4, item5, item6, item7
            };

            await Context.GameItem.AddRangeAsync(gameItems);
            
            //Create Game Case
            GameCase gameCase = new()
            {
                Id = gameCaseGuid,
                GroupCasesName = "GCNGOCATGroupName",
                GameCaseName = "GCNGOCATName",
                GameCaseCost = 400,
                GameCaseImage = "GCIGOCATImage",
                RevenuePrecentage = 0.1M
            };
            await Context.GameCase.AddAsync(gameCase);

            //Create CaseInventory
            gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();

            CaseInventory inventory1 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[0].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventory inventory2 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[1].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventory inventory3 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[2].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventory inventory4 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[3].Id,
                LossChance = 3000,
                NumberItemsCase = 1
            };
            CaseInventory inventory5 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[4].Id,
                LossChance = 7000,
                NumberItemsCase = 1
            };
            CaseInventory inventory6 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[5].Id,
                LossChance = 20545,
                NumberItemsCase = 1
            };
            CaseInventory inventory7 = new()
            {
                GameCaseId = gameCase.Id,
                GameItemId = gameItems[6].Id,
                LossChance = 20918,
                NumberItemsCase = 1
            };

            List<CaseInventory> caseInventories = new() {
                inventory1,
                inventory2,
                inventory3,
                inventory4,
                inventory5,
                inventory6,
                inventory7
            };

            await Context.CaseInventory.AddRangeAsync(caseInventories);

            await Context.SaveChangesAsync();
        }

        private async Task RemoveTestDependencies(List<Guid> gameItemsGuids, Guid gameCaseGuid)
        {
            foreach(Guid id in gameItemsGuids)
            {
                GameItem? gameItem = await Context.GameItem.FirstOrDefaultAsync(x => x.Id == id);
                Context.GameItem.Remove(gameItem!);
            }

            GameCase? gameCase = await Context.GameCase.FirstOrDefaultAsync(x => x.Id == gameCaseGuid);
            Context.GameCase.Remove(gameCase!);

            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
