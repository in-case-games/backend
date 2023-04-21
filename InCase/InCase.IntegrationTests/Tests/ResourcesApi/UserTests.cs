using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class UserTests: BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private readonly Dictionary<string, Guid> DependenciesGuids = new() {
            ["User"] = Guid.NewGuid(),
            ["Promocode"] = Guid.NewGuid(),
            ["GameItem"] = Guid.NewGuid(),
            ["LootBox"] = Guid.NewGuid(),
            ["LootBoxBanner"] = Guid.NewGuid(),
            ["UserHistoryPromocode"] = Guid.NewGuid(),
            ["UserHistoryWithdrawn"] = Guid.NewGuid(),
            ["UserHistoryOpening"] = Guid.NewGuid(),
            ["UserPathBanner"] = Guid.NewGuid(),
            ["UserInventory"] = Guid.NewGuid(),
            ["UserHistoryPayment"] = Guid.NewGuid(),
        };
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public UserTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output): base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Theory]
        [InlineData("/api/user/history/promocodes", HttpStatusCode.NotFound)]
        [InlineData("/api/user/history/withdrawns", HttpStatusCode.NotFound)]
        [InlineData("/api/user/history/openings", HttpStatusCode.NotFound)]
        [InlineData("/api/user/banners", HttpStatusCode.NotFound)]
        [InlineData("/api/user/inventory", HttpStatusCode.NotFound)]
        [InlineData("/api/history/payments", HttpStatusCode.NotFound)]
        public async Task GET_UserData_NotFound(string uri, HttpStatusCode statusCode, string userRole = "bot")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], userRole);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData("/api/user", HttpStatusCode.OK)]
        [InlineData("/api/user/history/promocodes", HttpStatusCode.OK)]
        [InlineData("/api/user/history/withdraws", HttpStatusCode.OK)]
        [InlineData("/api/user/history/openings", HttpStatusCode.OK)]
        [InlineData("/api/user/banners", HttpStatusCode.OK)]
        [InlineData("/api/user/inventory", HttpStatusCode.OK)]
        [InlineData("/api/user/history/payments", HttpStatusCode.OK, "user")]
        public async Task GET_UserData_OK(string uri, HttpStatusCode statusCode, string userRole = "bot")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], userRole);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData("/api/user", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/promocodes", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/withdraws", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/openings", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/banners", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/inventory", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/payments", HttpStatusCode.Unauthorized)]
        public async Task GET_UserData_Unauthorized(string uri, HttpStatusCode statusCode, string userRole = "bot")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], userRole);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(statusCode, getStatusCode);
        }
        [Fact]
        public async Task GET_UserMainData_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user/", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_UserDataById_OK()
        {
            // Arrange 
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_WithdrawHistoriesById_NotFoundUser()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/history/withdraws");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_WithdrawHistoriesById_NotFoundHistory()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/history/withdraws", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_UserInventoryById_NotFoundInventory()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/inventory");

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_UserInventoryById_NotFoundUser()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/inventory");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_WithdrawHistoriesById_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/history/withdraws", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_UserInventoryById_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/inventory");

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocode_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocodeNullNumberActivation_Conflict()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            promocode.NumberActivations = -1;
            Context.Promocodes.Update(promocode);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocodeTimeIsUp_Conflict()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            promocode.ExpirationDate = DateTime.UtcNow - TimeSpan.FromDays(5);
            Context.Promocodes.Update(promocode);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocode_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/fxckyousaw98134122", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocode_ConflictUsed()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocode_ConflictAlreadyUsed()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            historyPromocode.IsActivated = true;
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task GET_ActivatePromocode_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}");

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangePromocode_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);

            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            promocode.Id = Guid.NewGuid();
            promocode.Name = GenerateString(8);

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveDependencies();

            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangePromocodeNullNumberActivations_Conflict()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);

            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            promocode.Id = Guid.NewGuid();
            promocode.Name = GenerateString(8);
            promocode.NumberActivations = -1;

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveDependencies();

            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangePromocode_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/fxckyuopusdjfa213123", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangePromocode_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            Promocode promocode = await Context.Promocodes
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}");

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/{promocode.Name}");

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_SellLastOpeningItem_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/sell/{DependenciesGuids["GameItem"]}", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_SellItemByInventoryId_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/sell", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_SellItemByInventoryId_Unauthorized()
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["GameItem"]}/sell");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_SellItemByInventoryId_NotFoundInventory()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/inventory/{Guid.NewGuid()}/sell", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_SellItemByInventoryId_NotFoundUser()
        {
            // Arrange
            string fakeUserToken = await CreateFakeToken();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/inventory/{DependenciesGuids["GameItem"]}/sell", fakeUserToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_SellLastOpeningItem_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/sell/{DependenciesGuids["GameItem"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_SellLastOpeningItem_NotFoundUser()
        {
            // Arrange
            string fakeUserToken = await CreateFakeToken();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/sell/{DependenciesGuids["GameItem"]}", fakeUserToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_SellLastOpeningItem_NotFoundItems()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/sell/{Guid.NewGuid()}", AccessToken);

            // Assert
            await ClearTableData("Promocode", "GameItem", "LootBox", "User");
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/" +
                $"exchange/{DependenciesGuids["GameItem"]}");

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/" +
                $"exchange/{DependenciesGuids["GameItem"]}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_NotFoundInventory()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{Guid.NewGuid()}/" +
                $"exchange/{DependenciesGuids["GameItem"]}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_NotFoundItem()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/" +
                $"exchange/{Guid.NewGuid()}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_NotFoundUser()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();
            string fakeUserToken = await CreateFakeToken();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/" +
                $"exchange/{DependenciesGuids["GameItem"]}", fakeUserToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_ExchangeGameItem_ConflictDifferentCostException()
        {
            // Arrange
            Guid item2Guid = Guid.NewGuid();
            await InitializeUserDependency(DependenciesGuids["User"]);
            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            GameItem item1 = new()
            {
                Id = DependenciesGuids["GameItem"],
                Name = GenerateString(8),
                Cost = 1M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            GameItem item2 = new()
            {
                Id = item2Guid,
                Name = GenerateString(8),
                Cost = 1000M,
                ImageUri = "GOCSATImage1",
                RarityId = rarities.FirstOrDefault(f => f.Name == "pink")!.Id,
                TypeId = types.FirstOrDefault(f => f.Name == "pistol")!.Id,
                GameId = csgoGameId,
                QualityId = qualities.FirstOrDefault(f => f.Name == "minimal wear")!.Id
            };

            UserInventory inventory = new()
            {
                Id = DependenciesGuids["UserInventory"],
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = DependenciesGuids["User"],
                FixedCost = 0M
            };

            await Context.GameItems.AddRangeAsync(item1, item2);
            await Context.UserInventories.AddAsync(inventory);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/inventory/{DependenciesGuids["UserInventory"]}/" +
                $"exchange/{item2Guid}", AccessToken);

            // Assert
            Context.GameItems.RemoveRange(item1, item2);
            Context.UserInventories.Remove(inventory);
            await Context.SaveChangesAsync();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, getStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_ConflictAlreadyExisted()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_NotFoundItem()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);
            await Context.SaveChangesAsync();

            GameItem item = await Context.GameItems
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);
            item.Id = Guid.NewGuid();

            pathBanner.Item = item;
            pathBanner.ItemId = item.Id;

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_NotFoundBanner()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);
            await Context.SaveChangesAsync();

            LootBoxBanner banner = await Context.LootBoxBanners
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);
            banner.Id = Guid.NewGuid();

            pathBanner.Banner = banner;
            pathBanner.BannerId = banner.Id;

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_ConflictSmallestItemCost()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);

            GameItem item = await Context.GameItems
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);
            item.Cost = -123;

            await Context.SaveChangesAsync();

            pathBanner.Item = item;
            pathBanner.ItemId = item.Id;

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_ConflictBiggestThanHungredSteps()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);

            GameItem item = await Context.GameItems
               .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);
            item.Cost = 123455667;

            await Context.SaveChangesAsync();

            pathBanner.Item = item;
            pathBanner.ItemId = item.Id;

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Conflict, postStatusCode);
        }
        [Fact]
        public async Task POST_PathBanner_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user/banner", pathBanner.Convert(false));

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.Unauthorized, postStatusCode);
        }
        [Fact]
        public async Task DELETE_PathBanner_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user/banner/{DependenciesGuids["LootBoxBanner"]}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_PathBanner_NotFoundUserInfo()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();
            string fakeUserToken = await CreateFakeToken();
            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user/banner/{DependenciesGuids["LootBoxBanner"]}", fakeUserToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_PathBanner_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();
            UserPathBanner pathBanner = await Context.UserPathBanners
                .FirstAsync(f => f.Id == DependenciesGuids["UserPathBanner"]);
            Context.UserPathBanners.Remove(pathBanner);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user/banner/{DependenciesGuids["LootBoxBanner"]}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_PathBanner_Unauthorized()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user/banner/{DependenciesGuids["UserPathBanner"]}");

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Unauthorized, deleteStatusCode);
        }

        #region Начальные данные
        private async Task InitializeDependencies()
        {
            UpdateContext();
            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Name = GenerateString(8),
                NumberActivations = 999,
                Discount = 0.99999M,
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromHours(1),
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
                Id = DependenciesGuids["GameItem"],
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
                Id = DependenciesGuids["LootBox"],
                Name = GenerateString(8),
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxBanner boxBanner = new()
            {
                Id = DependenciesGuids["LootBoxBanner"],
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
                Id = DependenciesGuids["UserHistoryPromocode"],
                Date = DateTime.UtcNow,
                IsActivated = false,
                PromocodeId = promocode.Id,
                UserId = DependenciesGuids["User"]
            };

            UserHistoryWithdraw withdraw = new()
            {
                Id = DependenciesGuids["UserHistoryWithdrawn"],
                IdForMarket = 10000,
                Date = DateTime.UtcNow,
                FixedCost = 500M,
                ItemId = item1.Id,
                UserId = DependenciesGuids["User"],
                StatusId = (await Context.ItemWithdrawStatuses.FirstAsync(f => f.Name == "given")).Id,
                MarketId = (await Context.GameMarkets.FirstAsync(f => f.Name == "tm")).Id
            };

            UserHistoryOpening opening = new()
            {
                Id = DependenciesGuids["UserHistoryOpening"],
                BoxId = lootBox.Id,
                UserId = DependenciesGuids["User"],
                Date = DateTime.UtcNow,
                ItemId = item1.Id
            };

            UserPathBanner pathBanner = new()
            {
                Id = DependenciesGuids["UserPathBanner"],
                BannerId = boxBanner.Id,
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                NumberSteps = 1,
                UserId = DependenciesGuids["User"]
            };
            
            UserInventory inventory = new()
            {
                Id = DependenciesGuids["UserInventory"],
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = DependenciesGuids["User"],
                FixedCost = 12345M
            };

            UserHistoryPayment payment = new()
            {
                Id = DependenciesGuids["UserHistoryPayment"],
                Amount = 145M,
                Date = DateTime.UtcNow,
                UserId = DependenciesGuids["User"]
            };

            await Context.UserHistoryPayments.AddAsync(payment);
            await Context.UserHistoryPromocodes.AddAsync(historyPromocode);
            await Context.UserHistoryWithdraws.AddAsync(withdraw);
            await Context.UserHistoryOpenings.AddAsync(opening);
            await Context.UserPathBanners.AddAsync(pathBanner);
            await Context.UserInventories.AddAsync(inventory);

            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveDependencies()
        {
            UpdateContext();

            List<Promocode> promocodes = await Context.Promocodes
                .AsNoTracking()
                .ToListAsync();
            GameItem item = await Context.GameItems
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);
            LootBox box = await Context.LootBoxes
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);
            
            foreach(var promocode in promocodes)
            {
                Context.Promocodes.Remove(promocode);
            }

            Context.GameItems.Remove(item);
            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
