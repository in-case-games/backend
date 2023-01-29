using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit.Abstractions;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly ResponseHelper _clientApi;
        private Random random = new();

        public GameApiTest(
            WebApplicationFactory<Program> applicationFactory,
            ITestOutputHelper output)
        {
            _clientApi = new(applicationFactory.CreateClient());
            _output = output;
        }

        [Fact]
        public async Task GameOpeningCasesSpeedApiTest()
        {
            await CreateDependencies();

            Guid caseId = (await _clientApi
                .ResponseGet<List<GameCase>>("/GameCase/GetAll"))
                .FirstOrDefault(x => x.GameCaseName == "Балансный")!
                .Id;
            Guid userId = (await _clientApi
                .ResponseGet<User>($"/User/GetByLogin?login=testuser2&hash=123"))
                .Id;
            List<GameItem> gameItems = await _clientApi
                .ResponseGet<List<GameItem>>("/GameItem/GetAll");

            List<int> winIndexes = new();

            gameItems = gameItems.OrderByDescending(g => g.GameItemCost).ToList();
            gameItems.ForEach(x => winIndexes.Add(0));

            for (int i = 1; i <= 1000; i++)
            {
                GameItem winItem = await _clientApi
                    .ResponseGet<GameItem>($"/Game?userId={userId}&caseId={caseId}");
                GameCase gameCase = (await _clientApi
                .ResponseGet<List<GameCase>>("/GameCase/GetAll"))
                .FirstOrDefault(x => x.GameCaseName == "Балансный")!;
                _output.WriteLine($"{winItem.GameItemName} - {winItem.GameItemCost}\n" +
                    $"Баланс: {gameCase.GameCaseBalance} Профит: {i * gameCase.GameCaseCost * gameCase.RevenuePrecentage}");
            }

            await DeleteDependencies();
        }

        private async Task CreateDependencies()
        {
            GameItem item1 = new()
            {
                GameItemName = "USP-S | Cortex (Немного поношенное)",
                GameItemCost = 239.99M,
                GameItemImage = "GOCSATImage1",
                GameItemRarity = "Засекреченное"
            };
            GameItem item2 = new()
            {
                GameItemName = "AK-47 | Ice Coaled (Закалённое в боях)",
                GameItemCost = 752M,
                GameItemImage = "GOCSATImage2",
                GameItemRarity = "Засекреченное"
            };
            GameItem item3 = new()
            {
                GameItemName = "Перчатки «Гидра» | Изумруд (Закаленное в боях)",
                GameItemCost = 4211M,
                GameItemImage = "GOCSATImage3",
                GameItemRarity = "Экстраординарного типа"
            };
            GameItem item4 = new()
            {
                GameItemName = "Обмотки рук | Пустынный шемах (Закаленное в боях)",
                GameItemCost = 4857M,
                GameItemImage = "GOCSATImage4",
                GameItemRarity = "Экстраординарного типа"
            };
            GameItem item5 = new()
            {
                GameItemName = "Фальшион | Ночь (После полевых испытаний)",
                GameItemCost = 8000M,
                GameItemImage = "GOCSATImage5",
                GameItemRarity = "Тайное"
            };
            GameItem item6 = new()
            {
                GameItemName = "Glock-18 | Ласка (Немного поношенное)",
                GameItemCost = 66M,
                GameItemImage = "GOCSATImage6",
                GameItemRarity = "Армейское качество"
            };
            GameItem item7 = new()
            {
                GameItemName = "AWP | Ахерон (После полевых испытаний)",
                GameItemCost = 61M,
                GameItemImage = "GOCSATImage7",
                GameItemRarity = "Армейское качество"
            };
            List<GameItem> gameItems = new() { 
                item1, item2, item3, item4, item5, item6, item7
            };
            GameCase gameCase = new()
            {
                GroupCasesName = "Бедные кейсы2",
                GameCaseName = "Балансный",
                GameCaseCost = 400,
                GameCaseImage = "вфыв",
                RevenuePrecentage = 0.1M
            };

            //Create Item
            foreach (GameItem item in gameItems)
                await _clientApi.ResponsePost("/GameItem", item);

            //Create Case
            await _clientApi.ResponsePost("/GameCase", gameCase);

            //Search Case and Items Id
            Guid caseId = (await _clientApi
                .ResponseGet<List<GameCase>>("/GameCase/GetAll"))
                .FirstOrDefault(x => x.GameCaseName == "Балансный")!
                .Id;

            List<Guid> itemsGuid = new();
            gameItems = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAll");

            foreach (GameItem item in gameItems)
                itemsGuid.Add(item.Id);

            //Create CaseInventory
            CaseInventory inventory1 = new() {
                GameCaseId = caseId,
                GameItemId = gameItems[0].Id,
                LossChance = 3200,
                NumberItemsCase = 1
            };
            CaseInventory inventory2 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[1].Id,
                LossChance = 1500,
                NumberItemsCase = 1
            };
            CaseInventory inventory3 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[2].Id,
                LossChance = 294,
                NumberItemsCase = 1
            };
            CaseInventory inventory4 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[3].Id,
                LossChance = 250,
                NumberItemsCase = 1
            };
            CaseInventory inventory5 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[4].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventory inventory6 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[5].Id,
                LossChance = 20886,
                NumberItemsCase = 1
            };
            CaseInventory inventory7 = new()
            {
                GameCaseId = caseId,
                GameItemId = gameItems[6].Id,
                LossChance = 20000,
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
            foreach (CaseInventory inventory in caseInventories)
                await _clientApi.ResponsePost("/CaseInventory", inventory);

            //Create User
            User user = new()
            {
                UserLogin = "testuser2",
                UserEmail = "testuser2",
                UserImage = "testuser2",
                PasswordHash = "testuser2",
                PasswordSalt = "testuser2",
            };
            await _clientApi.ResponsePost("/User", user);
            user = await _clientApi
                .ResponseGet<User>($"/User/GetByLogin?login=testuser2&hash=123");

            //Create Additional Info
            UserAdditionalInfo userInfo = new()
            {
                UserId = user.Id,
                UserAge = 18,
                UserBalance = 9999999999,
                UserAbleToPay = 0
            };
            await _clientApi.ResponsePost("/UserAdditionalInfo", userInfo);
        }

        private async Task DeleteDependencies()
        {
            //Delete Case and Case Inventory 
            Guid caseId = (await _clientApi
                .ResponseGet<List<GameCase>>("/GameCase/GetAll"))
                .FirstOrDefault(x => x.GameCaseName == "Балансный")!
                .Id;
            await _clientApi.ResponseDelete($"/GameCase?id={caseId}");

            //Delete Items
            List<Guid> itemsGuid = new();
            List<GameItem> gameItems = await _clientApi
                .ResponseGet<List<GameItem>>("/GameItem/GetAll");

            foreach (GameItem item in gameItems)
                await _clientApi.ResponseDelete($"/GameItem?id={item.Id}");

            //Delete full info user
            Guid userId = (await _clientApi
                .ResponseGet<User>($"/User/GetByLogin?login=testuser2&hash=123"))
                .Id;
            await _clientApi.ResponseDelete($"/User?id={userId}");
        }
    }
}
