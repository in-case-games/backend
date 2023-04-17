using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Engines;
using System.Diagnostics;
using System.Net;
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
        private readonly List<Guid> ItemsGuids = new() {
            Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid()
        };
        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["LootBox"] = Guid.NewGuid(),
            ["LootBoxBanner"] = Guid.NewGuid(),
            ["Promocode"] = Guid.NewGuid(),
            ["UserHistoryPromocode"] = Guid.NewGuid()
        };
        private readonly Dictionary<string, decimal> ItemsAndCost = new()
        {
            ["USP-S - Cortex"] = 239.99M,
            ["AK-47 - Ice Coaled"] = 752M,
            ["Перчатки «Гидра» - Изумруд"] = 4211M,
            ["Обмотки рук - Пустынный шемах"] = 4857M,
            ["Фальшион - Ночь"] = 8000M,
            ["Glock-18 - Ласка"] = 66M,
            ["AWP - Ахерон"] = 61M,
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostGameApiTests>()
            .Build();

        public OpenLootBoxTests(
            WebApplicationFactory<HostGameApiTests> hostGame,
            WebApplicationFactory<HostResourcesApiTests> hostResources,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseGame = new(hostGame.CreateClient())
            {
                BaseUrl = "https://localhost:7139"
            };
            _responseResources = new(hostResources.CreateClient())
            {
                BaseUrl = "https://localhost:7102"
            };
        }

        [Theory]
        [InlineData("/api/open-loot-box/")]
        [InlineData("/api/open-loot-box/virtual/")]
        public async Task GET_OpeningLootBox_NotFoundBox(string uri)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode(uri + Guid.NewGuid(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Theory]
        [InlineData("/api/open-loot-box/")]
        [InlineData("/api/open-loot-box/virtual/")]
        public async Task GET_OpeningLootBox_NotFoundUser(string uri)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["User"]);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode(uri + DependenciesGuids["LootBox"], AccessToken);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Theory]
        [InlineData("/api/open-loot-box/virtual/", HttpStatusCode.OK)]
        [InlineData("/api/open-loot-box/", HttpStatusCode.Conflict)]
        public async Task GET_OpeningLootBox_ConflictAndOKLocked(string uri, HttpStatusCode result)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeTestDependencies();

            if (uri == "/api/open-loot-box/virtual/")
            {
                UserAdditionalInfo info = await Context.UserAdditionalInfos
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

                info.IsGuestMode = true;
            }

            LootBox box = await Context.LootBoxes
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);
            box.IsLocked = true;
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode(uri + DependenciesGuids["LootBox"], AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(result, statusCode);
        }

        [Theory]
        [InlineData("/api/open-loot-box/virtual/", HttpStatusCode.OK)]
        [InlineData("/api/open-loot-box/", HttpStatusCode.Conflict)]
        public async Task GET_OpeningLootBox_ConflictAndOKBalance(string uri, HttpStatusCode result)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user", 0M);
            await InitializeTestDependencies();

            if (uri == "/api/open-loot-box/virtual/")
            {
                UserAdditionalInfo info = await Context.UserAdditionalInfos
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

                info.IsGuestMode = true;

                await Context.SaveChangesAsync();
            }

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode(uri + DependenciesGuids["LootBox"], AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(result, statusCode);
        }

        [Fact]
        public async Task GET_OpeningLootBoxUsePromocode_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user", 400M);
            await InitializeTestDependencies();

            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Name = GenerateString(8),
                NumberActivations = 1,
                Discount = 0.1M,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.First(f => f.Name == "case")!.Id
            };

            UserHistoryPromocode historyPromocode = new()
            {
                Id = DependenciesGuids["UserHistoryPromocode"],
                IsActivated = false,
                UserId = DependenciesGuids["User"],
                PromocodeId = promocode.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            UserAdditionalInfo info = await Context.UserAdditionalInfos
                .AsNoTracking()
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, 40), 
                (statusCode, info.Balance));
        }

        [Fact]
        public async Task GET_OpeningLootBoxUsePromocodeFor99Percent_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user", 400M);
            await InitializeTestDependencies();

            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Name = GenerateString(8),
                NumberActivations = 1,
                Discount = 0.99M,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.First(f => f.Name == "case")!.Id
            };

            UserHistoryPromocode historyPromocode = new()
            {
                Id = DependenciesGuids["UserHistoryPromocode"],
                IsActivated = false,
                UserId = DependenciesGuids["User"],
                PromocodeId = promocode.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            UserAdditionalInfo info = await Context.UserAdditionalInfos
                .AsNoTracking()
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, 399),
                (statusCode, info.Balance));
        }

        [Fact]
        public async Task GET_OpeningLootBoxCanNotBeNevativeBalance_True()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner", 400M);
            await InitializeTestDependencies();

            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Name = GenerateString(8),
                NumberActivations = 1,
                Discount = 0.99M,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.First(f => f.Name == "case")!.Id
            };

            UserHistoryPromocode historyPromocode = new()
            {
                Id = DependenciesGuids["UserHistoryPromocode"],
                IsActivated = false,
                UserId = DependenciesGuids["User"],
                PromocodeId = promocode.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            SiteStatisticsAdmin statisticsAdmin = await Context.SiteStatisticsAdmins
                .FirstAsync();

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.True(
                HttpStatusCode.OK == statusCode && 
                box.Balance >= 0 && 
                statisticsAdmin.BalanceWithdrawn < 0);
        }

        [Theory]
        [InlineData("/api/open-loot-box/")]
        [InlineData("/api/open-loot-box/virtual/")]
        public async Task GET_OpeningLootBox_Unauthorized(string uri)
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode(uri + DependenciesGuids["LootBox"]);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Fact]
        public async Task GET_OpeningLootBoxAlreadyUsePromocode_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user", 400M);
            await InitializeTestDependencies();

            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Name = GenerateString(8),
                NumberActivations = 1,
                Discount = 0.99M,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.First(f => f.Name == "case")!.Id
            };

            UserHistoryPromocode historyPromocode = new()
            {
                Id = DependenciesGuids["UserHistoryPromocode"],
                IsActivated = true,
                UserId = DependenciesGuids["User"],
                PromocodeId = promocode.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            UserAdditionalInfo info = await Context.UserAdditionalInfos
                .AsNoTracking()
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, 0),
                (statusCode, info.Balance));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GET_OpeningLootBox_Output(bool IsVirtual)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            //Act
            if(IsVirtual)
            {
                UserAdditionalInfo info = await Context.UserAdditionalInfos
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

                info.IsGuestMode = true;
                await Context.SaveChangesAsync();
            }

            //Act & Assert
            await RunOpenLootBoxTest(IsVirtual);

            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();
        }

        [Fact]
        public async Task GET_OpeningLootBoxSubcribedBannerNoActive_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();
            await InitializeDependeciesForBanner(2);

            UpdateContext();

            UserPathBanner pathBeforeOpening = await Context.UserPathBanners
                .AsNoTracking()
                .FirstAsync(f => f.BannerId == DependenciesGuids["LootBoxBanner"]);

            LootBoxBanner banner = await Context.LootBoxBanners
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            banner.IsActive = false;

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            UserPathBanner pathAfterOpening = await Context.UserPathBanners
                .AsNoTracking()
                .FirstAsync(f => f.BannerId == DependenciesGuids["LootBoxBanner"]);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, pathBeforeOpening.NumberSteps), 
                (statusCode, pathAfterOpening.NumberSteps));
        }

        [Fact]
        public async Task GET_OpeningLootBoxSubcribedBanner_Output()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();
            await InitializeDependeciesForBanner(2);

            //Act & Assert
            await RunOpenLootBoxTest(false, 2);

            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();
        }

        [Fact]
        public async Task GET_OpeningVirtualLootBox_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/virtual/{DependenciesGuids["LootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        [Fact]
        public async Task GET_OpeningVirtualLootBoxCheckVirtualBalance_True()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin", 400);
            await InitializeTestDependencies();

            UserAdditionalInfo info = await Context.UserAdditionalInfos
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

            info.IsGuestMode = true;
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseGame
                .ResponseGetStatusCode($"/api/open-loot-box/virtual/{DependenciesGuids["LootBox"]}", AccessToken);

            UpdateContext();

            info = await Context.UserAdditionalInfos
                .AsNoTracking()
                .FirstAsync(f => f.UserId == DependenciesGuids["User"]);

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.True(
                HttpStatusCode.OK == statusCode && 
                box.VirtualBalance > 0 && 
                info.Balance == 400 &&
                box.Balance == 0);
        }


        #region Начальные данные
        private async Task RunOpenLootBoxTest(bool IsVirtual = false, int pathItemIndex = -1, int numberOpening = 1000)
        {
            //Act
            decimal allCostItems = 0;
            Stopwatch startTime = Stopwatch.StartNew();
            Dictionary<string, int> winingItems = await GetWiningItems(IsVirtual, pathItemIndex, numberOpening);

            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = string.Format("{0:00}.{1:000}",
                resultTime.Seconds,
                resultTime.Milliseconds);

            //Assert
            _output.WriteLine("Предмет - количество - стоимость лута");

            foreach (var winItem in winingItems)
            {
                decimal costWiningItems = ItemsAndCost[winItem.Key] * winItem.Value;
                allCostItems += costWiningItems;
                _output.WriteLine($"{winItem.Key} - {winItem.Value} - {costWiningItems}");
            }

            LootBox? lootBox = (await _responseResources
                .ResponseGet<AnswerBoxApi?>($"/api/loot-box/{DependenciesGuids["LootBox"]}/admin",
                AccessToken))!.Data;

            SiteStatisticsAdmin statisticsAdmin = await Context.SiteStatisticsAdmins
                .FirstAsync();

            _output.WriteLine(
                $"Профит сайта: {statisticsAdmin.BalanceWithdrawn} Р\n" +
                $"Баланс кейса: {lootBox!.Balance} Р\n" +
                $"Стоимость всех предметов: {allCostItems}\n" +
                $"Скорость алгоритма: {elapsedTime}");
        }
        private async Task<Dictionary<string, int>> GetWiningItems(bool IsVirtual = false, int pathItemIndex = -1, int numberOpening = 1000)
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

            for (int i = 0; i < numberOpening; i++)
            {
                GameItem? winItem = null;

                //Open Cases
                if (IsVirtual)
                {
                    winItem = (await _responseGame
                        .ResponseGet<AnswerItemApi?>($"/api/open-loot-box/virtual/{DependenciesGuids["LootBox"]}",
                        AccessToken))?.Data;
                }
                else
                {
                    winItem = (await _responseGame
                        .ResponseGet<AnswerItemApi?>($"/api/open-loot-box/{DependenciesGuids["LootBox"]}",
                        AccessToken))?.Data;
                }

                winItem = winItem ?? throw new Exception("No opened cases");

                //Counter wins
                winItems[winItem!.Name!] += 1;

                if(pathItemIndex >= 0)
                {
                    UserPathBannerDto pathBannerDto = new()
                    {
                        BannerId = DependenciesGuids["LootBoxBanner"],
                        Date = DateTime.UtcNow,
                        ItemId = ItemsGuids[pathItemIndex],
                        NumberSteps = 1,
                        UserId = DependenciesGuids["User"]
                    };
                    
                    await _responseResources
                        .ResponsePostStatusCode("/api/user/banner", pathBannerDto, AccessToken);
                }
            }

            return winItems;
        }

        private async Task InitializeDependeciesForBanner(int itemIndex)
        {
            UpdateContext();

            LootBoxBanner boxBanner = new()
            {
                Id = DependenciesGuids["LootBoxBanner"],
                BoxId = DependenciesGuids["LootBox"],
                IsActive = true,
                ImageUri = "",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(7)
            };

            UserPathBannerDto pathBannerDto = new()
            {
                BannerId = DependenciesGuids["LootBoxBanner"],
                Date = DateTime.UtcNow,
                ItemId = ItemsGuids[itemIndex],
                NumberSteps = 1,
                UserId = DependenciesGuids["User"]
            };

            await Context.LootBoxBanners.AddAsync(boxBanner);
            await Context.SaveChangesAsync();

            await _responseResources
                .ResponsePostStatusCode("/api/user/banner", pathBannerDto, AccessToken);
        }

        private async Task InitializeTestDependencies()
        {
            UpdateContext();
            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            //Create Game Item
            GameItem item1 = new()
            {
                Id = ItemsGuids[0],
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
                Id = ItemsGuids[1],
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
                Id = ItemsGuids[2],
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
                Id = ItemsGuids[3],
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
                Id = ItemsGuids[4],
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
                Id = ItemsGuids[5],
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
                Id = ItemsGuids[6],
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
                Id = DependenciesGuids["LootBox"],
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

        private async Task RemoveTestDependencies()
        {
            UpdateContext();
            foreach (Guid id in ItemsGuids)
            {
                GameItem? gameItem = await Context.GameItems
                    .FirstOrDefaultAsync(f => f.Id == id);
                Context.GameItems.Remove(gameItem!);
            }

            LootBox? lootBox = await Context.LootBoxes
                .FirstOrDefaultAsync(f => f.Id == DependenciesGuids["LootBox"]);
            SiteStatisticsAdmin statisticsAdmin = await Context.SiteStatisticsAdmins
                .FirstAsync();
            SiteStatistics statistics = await Context.SiteStatistics
                .FirstAsync();
            Context.LootBoxes.Remove(lootBox!);
            statisticsAdmin.BalanceWithdrawn = 0;
            statistics.LootBoxes = 0;
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
