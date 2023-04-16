using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class UserApiTest: BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public UserApiTest(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output): base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Theory]
        [InlineData("/api/user", HttpStatusCode.OK)]
        [InlineData("/api/user/history/promocodes", HttpStatusCode.NotFound)]
        [InlineData("/api/user/history/withdrawns", HttpStatusCode.NotFound)]
        [InlineData("/api/user/history/openings", HttpStatusCode.NotFound)]
        [InlineData("/api/user/banners", HttpStatusCode.NotFound)]
        [InlineData("/api/user/inventory", HttpStatusCode.NotFound)]
        [InlineData("/api/history/payments", HttpStatusCode.NotFound)]
        public async Task GET_GetEmptyUserData_ReturnsOkOrNotFound(string uri, HttpStatusCode statusCode, string userRole = "bot")
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid, userRole);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            Assert.Equal(statusCode, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Theory]
        [InlineData("/api/user/history/promocodes", HttpStatusCode.OK)]
        [InlineData("/api/user/history/withdrawns", HttpStatusCode.OK)]
        [InlineData("/api/user/history/openings", HttpStatusCode.OK)]
        [InlineData("/api/user/banners", HttpStatusCode.OK)]
        [InlineData("/api/user/inventory", HttpStatusCode.OK)]
        [InlineData("/api/user/history/payments", HttpStatusCode.OK, "user")]
        public async Task GET_GetUserData_ReturnsOk(string uri, HttpStatusCode statusCode, string userRole = "bot")
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid, userRole);
            await InitializeDependencies(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            Assert.Equal(statusCode, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetUserById_ReturnsOk()
        {
            // Arrange 
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetEmptyWithdrawnHistoriesById_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/history/withdrawns", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetEmptyUserInventoryById_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/inventory");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetWithdrawnHistoriesById_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);
            await InitializeDependencies(guid);

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/history/withdrawns", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetUserInventoryById_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);
            await InitializeDependencies(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/inventory");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_ActivatePromocode_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            Promocode promocode = new()
            {
                Name = GenerateString(8),
                NumberActivations = 999,
                Discount = 999,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.FirstOrDefault(f => f.Name == "case")!.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_ExchangePromocode_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            Promocode promocode = new()
            {
                Name = GenerateString(8),
                NumberActivations = 999,
                Discount = 999,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.FirstOrDefault(f => f.Name == "case")!.Id
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/{promocode.Name}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            GameItem item1 = new()
            {
                Name = GenerateString(8),
                Cost = 239.99M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            UserInventory inventory = new()
            {
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = guid,
                FixedCost = 12345M
            };

            await Context.GameItems.AddAsync(item1);
            await Context.UserInventories.AddAsync(inventory);
            await Context.SaveChangesAsync();


            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{inventory.Id}/exchange/{item1.Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task POST_CreatePathBanner_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            GameItem item1 = new()
            {
                Name = GenerateString(8),
                Cost = 539.99M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            //Create Game Case
            LootBox lootBox = new()
            {
                Name = GenerateString(8),
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxBanner boxBanner = new()
            {
                BoxId = lootBox.Id,
                IsActive = true,
                ImageUri = "",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(7)
            };

            UserPathBannerDto pathBannerDto = new()
            {
                BannerId = boxBanner.Id,
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                NumberSteps = 1,
                UserId = guid
            };

            await Context.GameItems.AddAsync(item1);
            await Context.LootBoxes.AddAsync(lootBox);
            await Context.LootBoxBanners.AddAsync(boxBanner);

            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBannerDto, AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, postStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task DELETE_RemovePathBanner_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            GameItem item1 = new()
            {
                Name = GenerateString(8),
                Cost = 539.99M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            //Create Game Case
            LootBox lootBox = new()
            {
                Name = GenerateString(8),
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxBanner boxBanner = new()
            {
                BoxId = lootBox.Id,
                IsActive = true,
                ImageUri = "",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(7)
            };

            UserPathBannerDto pathBannerDto = new()
            {
                BannerId = boxBanner.Id,
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                NumberSteps = 1,
                UserId = guid
            };

            await Context.GameItems.AddAsync(item1);
            await Context.LootBoxes.AddAsync(lootBox);
            await Context.LootBoxBanners.AddAsync(boxBanner);
            await Context.UserPathBanners.AddAsync(pathBannerDto.Convert());

            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user/banner/{pathBannerDto.BannerId}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        #region Начальные данные
        private async Task InitializeDependencies(Guid userId)
        {
            Promocode promocode = new()
            {
                Name = GenerateString(8),
                NumberActivations = 999,
                Discount = 999,
                ExpirationDate = DateTime.UtcNow,
                TypeId = Context!.PromocodeTypes!.FirstOrDefault(f => f.Name == "balance")!.Id
            };

            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            //Create Game Item
            GameItem item1 = new()
            {
                Name = GenerateString(8),
                Cost = 239.99M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            //Create Game Case
            LootBox lootBox = new()
            {
                Name = GenerateString(8),
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxBanner boxBanner = new()
            {
                BoxId = lootBox.Id,
                IsActive = true,
                ImageUri = "",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(7)
            };

            await Context.LootBoxBanners.AddAsync(boxBanner);
            await Context.GameItems.AddAsync(item1);
            await Context.LootBoxes.AddAsync(lootBox);
            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            UserHistoryPromocode historyPromocode = new()
            {
                Date = DateTime.UtcNow,
                IsActivated = false,
                PromocodeId = promocode.Id,
                UserId = userId
            };

            UserHistoryWithdrawn withdrawn = new()
            {
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = userId
            };

            UserHistoryOpening opening = new()
            {
                BoxId = lootBox.Id,
                UserId = userId,
                Date = DateTime.UtcNow,
                ItemId = item1.Id
            };

            UserPathBannerDto pathBannerDto = new()
            {
                BannerId = boxBanner.Id,
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                NumberSteps = 1,
                UserId = userId
            };
            
            UserInventory inventory = new()
            {
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = userId,
                FixedCost = 12345M
            };

            UserHistoryPayment payment = new()
            {
                Amount = 145M,
                Date = DateTime.UtcNow,
                UserId = userId
            };

            await Context.UserHistoryPayments.AddAsync(payment);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.UserHistoryWithdrawns.AddAsync(withdrawn);
            await Context.UserHistoryOpenings.AddAsync(opening);
            await Context.UserPathBanners.AddAsync(pathBannerDto.Convert());
            await Context.UserInventories.AddAsync(inventory);

            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
