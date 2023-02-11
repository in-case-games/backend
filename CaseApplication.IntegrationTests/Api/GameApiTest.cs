using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Dtos;
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
        private readonly AuthenticationTestHelper _authHelper;
        private TokenModel AdminTokens { get; set; } = new();
        private User Admin { get; set; } = new();
        private GameCaseDto GameCase { get; set; } = new();
        private List<GameItem> GameItems { get; set; } = new();

        public GameApiTest(
            WebApplicationFactory<Program> applicationFactory,
            ITestOutputHelper output)
        {
            _clientApi = new(applicationFactory.CreateClient());
            _authHelper = new AuthenticationTestHelper(_clientApi);
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

        private async Task InitializeOneTimeAccount(string token)
        {
            Admin = new()
            {
                UserEmail = "yt_ferbray@mail.ru",
                UserLogin = "GIS"
            };

            AdminTokens = await _authHelper.SignInSuperAdmin(token);
        }

        private async Task DeleteOneTimeAccounts(string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULGOCAT{ipAdmin}Admin", AdminTokens.AccessToken!);
        }

        [Fact]
        public async Task GameOpeningCasesApiTest()
        {
            await InitializeOneTimeAccount("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImY2YzgxMmYzLWIxMTMtNGI1NS1iOTUwLTE3OTQ0NGEzMzBjNSIsImV4cCI6MTY3NjcwNDk2MCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA1MyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNTMifQ.TqyOccgBEblg_V3Pm3BpxMA6mUPD4ulBFJNJbO666PE");
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
                    .ResponseGet<GameItem?>($"/Game/{GameCase.Id}", AdminTokens.AccessToken!);

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

            GameCase gameCase = (await _clientApi.ResponseGet<GameCase?>($"/GameCase/admin/{GameCase.Id}", AdminTokens.AccessToken!))!;
            
            _output.WriteLine($"Баланс: {gameCase.GameCaseBalance}\n" +
                $"Скорость алгоритма: {elapsedTime}");

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
            GameItems = new() { 
                item1, item2, item3, item4, item5, item6, item7
            };

            //Create Item
            foreach (GameItem item in GameItems)
                await _clientApi.ResponsePostStatusCode("/GameItem/admin", item, AdminTokens.AccessToken!);

            //Create Case
            await _clientApi.ResponsePostStatusCode("/GameCase/admin", GameCase, AdminTokens.AccessToken!);

            //Search Case and Items Id
            GameCase.Id = await SearchIdCaseByName(GameCase.GameCaseName!);
            GameItems = GameItems.OrderByDescending(g => g.GameItemCost).ToList();

            for(int i = 0; i < GameItems.Count; i++)
            {
                GameItem? searchItem = await _clientApi.ResponseGet<GameItem?>(
                    $"/GameItem/name/{GameItems[i].GameItemName}");

                if (searchItem != null)
                {
                    GameItems[i].Id = searchItem.Id;
                }
            }

            //Create CaseInventory
            CaseInventoryDto inventory1 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[0].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory2 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[1].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory3 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[2].Id,
                LossChance = 1000,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory4 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[3].Id,
                LossChance = 3000,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory5 = new() {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[4].Id,
                LossChance = 7000,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory6 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[5].Id,
                LossChance = 20545,
                NumberItemsCase = 1
            };
            CaseInventoryDto inventory7 = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItems[6].Id,
                LossChance = 20918,
                NumberItemsCase = 1
            };

            List<CaseInventoryDto> caseInventories = new() { 
                inventory1, 
                inventory2, 
                inventory3, 
                inventory4, 
                inventory5, 
                inventory6, 
                inventory7
            };

            foreach (CaseInventoryDto inventory in caseInventories)
            {
                await _clientApi.ResponsePostStatusCode(
                    "/CaseInventory/admin", inventory, AdminTokens.AccessToken!);
            }
        }
        private async Task<Guid> SearchIdCaseByName(string name)
        {
            GameCase? gameCase = await _clientApi.ResponseGet<GameCase?>($"/GameCase/name/{name}");

            if (gameCase == null) throw new Exception("No such case :)");

            return gameCase.Id;
        }

        private async Task DeleteDependencies()
        {
            //Delete Case and Case Inventory
            await _clientApi.ResponseDelete($"/GameCase/admin/{GameCase.Id}", AdminTokens.AccessToken!);

            //Delete Items
            foreach (GameItem item in GameItems)
                await _clientApi.ResponseDelete($"/GameItem/admin/{item.Id}", AdminTokens.AccessToken!);
        }
    }
}
