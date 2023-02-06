using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using Xunit.Abstractions;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private GameCase GameCase { get; set; } = new();
        private List<GameItem> GameItems { get; set; } = new();

        public GameApiTest(
            WebApplicationFactory<Program> applicationFactory,
            ITestOutputHelper output)
        {
            _clientApi = new(applicationFactory.CreateClient());
            _output = output;

            GameCase = new()
            {
                GroupCasesName = "GCNGOCATGroupName",
                GameCaseName = "GCNGOCATName",
                GameCaseCost = 400,
                GameCaseImage = "GCIGOCATImage",
                RevenuePrecentage = 0.1M
            };
        }

        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULGOCAT{ipUser}User",
                UserEmail = $"UEGOCAT{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULGOCAT{ipAdmin}Admin",
                UserEmail = $"UEGOCAT{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULGOCAT{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULGOCAT{ipAdmin}Admin");
        }

        [Fact]
        public async Task GameOpeningCasesApiTest()
        {
            await InitializeOneTimeAccounts("0.3.0", "0.3.1");
            await CreateDependencies();

            //Create Counter
            List<int> winIndexes = new();
            GameItems.ForEach(x => winIndexes.Add(0));

            //Start Time
            Stopwatch startTime = Stopwatch.StartNew();

            for (int i = 1; i <= 1000; i++)
            {
                //Open Cases
                GameItem? winItem = await _clientApi
                    .ResponseGet<GameItem?>($"/Game?caseId={GameCase.Id}", AdminTokens.AccessToken!);

                if (winItem == null) throw new Exception("No opened cases"); 

                //Counter wins
                for (int j = 0; j < GameItems.Count; j++)
                {
                    if (GameItems[j].GameItemName == winItem.GameItemName)
                        winIndexes[j]++;
                }
                /*
                  _output.WriteLine($"{winItem.GameItemName} - {winItem.GameItemCost}\n" +
                  $"Баланс: {gameCase.GameCaseBalance} Профит: {i * gameCase.GameCaseCost * gameCase.RevenuePrecentage}");
                */
            }

            //End Time
            startTime.Stop();
            var resultTime = startTime.Elapsed;

            //Output
            for (int i = 0; i < winIndexes.Count; i++)
                _output.WriteLine($"{GameItems[i].GameItemName} - {winIndexes[i]}");

            string elapsedTime = String.Format("{0:00}.{1:000}",
                resultTime.Seconds,
                resultTime.Milliseconds);

            _output.WriteLine($"Баланс: {GameCase.GameCaseBalance}\n" +
                $"Скорость алгоритма: {elapsedTime}");

            await DeleteDependencies();

            await DeleteOneTimeAccounts("0.3.0", "0.3.1");
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
            GameItems = new() { 
                item1, item2, item3, item4, item5, item6, item7
            };

            //Create Item
            foreach (GameItem item in GameItems)
                await _clientApi.ResponsePostStatusCode("/GameItem", item, AdminTokens.AccessToken!);

            //Create Case
            await _clientApi.ResponsePostStatusCode("/GameCase", GameCase, AdminTokens.AccessToken!);

            //Search Case and Items Id
            GameCase.Id = await SearchIdCaseByName(GameCase.GameCaseName!);
            GameItems = GameItems.OrderByDescending(g => g.GameItemCost).ToList();

            for(int i = 0; i < GameItems.Count; i++)
            {
                GameItem? searchItem = await _clientApi.ResponseGet<GameItem?>(
                    $"/GameItem/GetByName?name={GameItems[i].GameItemName}");
                if (searchItem != null)
                {
                    GameItems[i].Id = searchItem.Id;
                }
            }

            //Create CaseInventory
            CaseInventory inventory1 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[0].Id,
                LossChance = 375,
                NumberItemsCase = 1
            };
            CaseInventory inventory2 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[1].Id,
                LossChance = 309,
                NumberItemsCase = 1
            };
            CaseInventory inventory3 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[2].Id,
                LossChance = 356,
                NumberItemsCase = 1
            };
            CaseInventory inventory4 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[3].Id,
                LossChance = 3989,
                NumberItemsCase = 1
            };
            CaseInventory inventory5 = new() {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[4].Id,
                LossChance = 10250,
                NumberItemsCase = 1
            };
            CaseInventory inventory6 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[5].Id,
                LossChance = 20545,
                NumberItemsCase = 1
            };
            CaseInventory inventory7 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[6].Id,
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

            foreach (CaseInventory inventory in caseInventories)
            {
                await _clientApi.ResponsePostStatusCode(
                    "/CaseInventory", inventory, AdminTokens.AccessToken!);
            }
        }

        private async Task DeleteDependencies()
        {
            //Delete Case and Case Inventory
            await _clientApi.ResponseDelete($"/GameCase?id={GameCase.Id}");

            //Delete Items
            foreach (GameItem item in GameItems)
                await _clientApi.ResponseDelete($"/GameItem?id={item.Id}");
        }

        private async Task<Guid> SearchIdCaseByName(string name)
        {
            GameCase? gameCase = await _clientApi.ResponseGet<GameCase?>(
                "/GameCase/GetByName?" +
                $"name={name}");

            if (gameCase == null) throw new Exception("No such case :)");

            return gameCase.Id;
        }
    }
}
