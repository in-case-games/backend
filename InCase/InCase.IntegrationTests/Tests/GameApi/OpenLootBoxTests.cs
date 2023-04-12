using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.GameApi
{
    public class OpenLootBoxTests : 
        BaseApiTest, 
        IClassFixture<WebApplicationFactory<HostGameApiTests>>, 
        IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        protected readonly ResponseService _responseGame;
        protected readonly ResponseService _responseResources;

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostGameApiTests>()
            .Build();

        public OpenLootBoxTests(
            WebApplicationFactory<HostGameApiTests> hostGame,
            WebApplicationFactory<HostResourcesApiTests> hostResources,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseGame = new ResponseService(hostGame.CreateClient());
            _responseGame.BaseUrl = "https://localhost:7139";
            _responseResources = new ResponseService(hostResources.CreateClient());
            _responseResources.BaseUrl = "https://localhost:7102";
        }

        [Fact]
        public async Task GET_OpeningLootBox_Output()
        {
            //Arrange
            Guid caseGuid = Guid.NewGuid();
            List<Guid> gameItemsGuids = new() {
                Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(),
            };

            await InitializeTestDependencies(gameItemsGuids, caseGuid);

            //Act
            Stopwatch startTime = Stopwatch.StartNew();
            Dictionary<string, int> winingItems = await GetWiningItems(caseGuid);
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = string.Format("{0:00}.{1:000}",
                resultTime.Seconds,
                resultTime.Milliseconds);

            //Assert
            _output.WriteLine("Предмет = количество выпадений");

            
            foreach (var winItem in winingItems)
            {
                _output.WriteLine($"{winItem.Key} = {winItem.Value}");
            }

            LootBox? lootBox = (await _responseResources
                .ResponseGet<AnswerBoxApi?>($"/api/loot-box/admin/{caseGuid}", AccessToken))!.Data;

            _output.WriteLine(
                $"Профит: {lootBox!.Cost * 0.1M * 1000} Р\n" +
                $"Баланс: {lootBox.Balance} Р\n" +
                $"Скорость алгоритма: {elapsedTime}");

            await RemoveTestDependencies(gameItemsGuids, caseGuid);
        }

        #region Начальные данные
        private async Task<Dictionary<string, int>> GetWiningItems(Guid gameCaseGuid)
        {
            Dictionary<string, int> winItems = new()
            {
                ["USP-S - Cortex"] = 0,
                ["AK-47 - Ice Coaled"] = 0,
                ["Перчатки «Гидра» - Изумруд"] = 0,
                ["Обмотки рук - Пустынный шемах"] = 0,
                ["Фальшион - Ночь"] = 0,
                ["Glock-18 - Ласка"] = 0,
                ["AWP - Ахерон"] = 0,
            };

            for (int i = 0; i <= 1000; i++)
            {
                //Open Cases
                GameItem? winItem = (await _responseGame
                    .ResponseGet<AnswerItemApi?>($"/api/open-loot-box/{gameCaseGuid}", AccessToken) ?? 
                    throw new Exception("No opened cases")).Data;

                //Counter wins
                winItems[winItem!.Name!] += 1;
            }

            return winItems;
        }

        private async Task InitializeTestDependencies(List<Guid> gameItemsGuids, Guid gameCaseGuid)
        {
            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            //Create Game Item
            GameItem item1 = new()
            {
                Id = gameItemsGuids[0],
                Name = "USP-S - Cortex",
                Cost = 239.99M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };
            GameItem item2 = new()
            {
                Id = gameItemsGuids[1],
                Name = "AK-47 - Ice Coaled",
                Cost = 752M,
                ImageUri = "GOCSATImage2",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "weapon")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "battle scarred")!.Id
            };
            GameItem item3 = new()
            {
                Id = gameItemsGuids[2],
                Name = "Перчатки «Гидра» - Изумруд",
                Cost = 4211M,
                ImageUri = "GOCSATImage3",
                RarityId = rarities.FirstOrDefault(f => f.Name == "gold")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "gloves")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "battle scarred")!.Id
            };
            GameItem item4 = new()
            {
                Id = gameItemsGuids[3],
                Name = "Обмотки рук - Пустынный шемах",
                Cost = 4857M,
                ImageUri = "GOCSATImage4",
                RarityId = rarities.FirstOrDefault(f => f.Name == "gold")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "gloves")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "battle scarred")!.Id
            };
            GameItem item5 = new()
            {
                Id = gameItemsGuids[4],
                Name = "Фальшион - Ночь",
                Cost = 8000M,
                ImageUri = "GOCSATImage5",
                RarityId = rarities.FirstOrDefault(f => f.Name == "gold")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "knife")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "field tested")!.Id
            };
            GameItem item6 = new()
            {
                Id = gameItemsGuids[5],
                Name = "Glock-18 - Ласка",
                Cost = 66M,
                ImageUri = "GOCSATImage6",
                RarityId = rarities.FirstOrDefault(f => f.Name == "blue")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };
            GameItem item7 = new()
            {
                Id = gameItemsGuids[6],
                Name = "AWP - Ахерон",
                Cost = 61M,
                ImageUri = "GOCSATImage7",
                RarityId = rarities.FirstOrDefault(f => f.Name == "blue")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "rifle")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "field tested")!.Id
            };
            List<GameItem> gameItems = new() {
                item1, item2, item3, item4, item5, item6, item7
            };

            await Context.GameItems.AddRangeAsync(gameItems);

            //Create Game Case
            LootBox lootBox = new()
            {
                Id = gameCaseGuid,
                Name = "GCNGOCATName",
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };
            await Context.LootBoxes.AddAsync(lootBox);

            //Create CaseInventory
            gameItems = gameItems.OrderByDescending(g => g.Cost).ToList();

            LootBoxInventory inventory1 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[0].Id,
                ChanceWining = 1000,
                NumberItems = 1
            };
            LootBoxInventory inventory2 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[1].Id,
                ChanceWining = 1000,
                NumberItems = 1
            };
            LootBoxInventory inventory3 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[2].Id,
                ChanceWining = 1000,
                NumberItems = 1
            };
            LootBoxInventory inventory4 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[3].Id,
                ChanceWining = 3000,
                NumberItems = 1
            };
            LootBoxInventory inventory5 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[4].Id,
                ChanceWining = 7000,
                NumberItems = 1
            };
            LootBoxInventory inventory6 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[5].Id,
                ChanceWining = 20545,
                NumberItems = 1
            };
            LootBoxInventory inventory7 = new()
            {
                BoxId = lootBox.Id,
                ItemId = gameItems[6].Id,
                ChanceWining = 20918,
                NumberItems = 1
            };

            List<LootBoxInventory> caseInventories = new() {
                inventory1,
                inventory2,
                inventory3,
                inventory4,
                inventory5,
                inventory6,
                inventory7
            };

            await Context.LootBoxInventories.AddRangeAsync(caseInventories);

            await Context.SaveChangesAsync();
        }

        private async Task RemoveTestDependencies(List<Guid> gameItemsGuids, Guid gameCaseGuid)
        {
            foreach (Guid id in gameItemsGuids)
            {
                GameItem? gameItem = await Context.GameItems.FirstOrDefaultAsync(x => x.Id == id);
                Context.GameItems.Remove(gameItem!);
            }

            LootBox? lootBox = await Context.LootBoxes.FirstOrDefaultAsync(x => x.Id == gameCaseGuid);
            Context.LootBoxes.Remove(lootBox!);

            await Context.SaveChangesAsync();
        }
        #endregion
        
        private class AnswerItemApi
        {
            public bool Success { get; set; }
            public GameItem? Data { get; set; }
        }

        private class AnswerBoxApi
        {
            public bool Success { get; set; }
            public LootBox? Data { get; set; }
        }
    }
}
