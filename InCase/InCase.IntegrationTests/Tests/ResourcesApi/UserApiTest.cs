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

        public UserApiTest(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
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
        public async Task GET_GetUserData_NotFound(string uri, HttpStatusCode statusCode, string userRole = "bot")
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
        [InlineData("/api/user/history/withdrawns", HttpStatusCode.OK)]
        [InlineData("/api/user/history/openings", HttpStatusCode.OK)]
        [InlineData("/api/user/banners", HttpStatusCode.OK)]
        [InlineData("/api/user/inventory", HttpStatusCode.OK)]
        [InlineData("/api/user/history/payments", HttpStatusCode.OK, "user")]
        public async Task GET_GetUserData_OK(string uri, HttpStatusCode statusCode, string userRole = "bot")
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
        [InlineData("/api/user/history/withdrawns", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/openings", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/banners", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/inventory", HttpStatusCode.Unauthorized)]
        [InlineData("/api/user/history/payments", HttpStatusCode.Unauthorized)]
        public async Task GET_GetUserData_Unauthorized(string uri, HttpStatusCode statusCode, string userRole = "bot")
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
        public async Task GET_GetUserMainData_NotFound()
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
        public async Task GET_GetUserDataById_OK()
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
        public async Task GET_GetWithdrawnHistoriesById_NotFoundUser()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{guid}/history/withdrawns");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_GetWithdrawnHistoriesById_NotFoundHistory()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/history/withdrawns", AccessToken);

            // Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_GetUserInventoryById_NotFoundInventory()
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
        public async Task GET_GetUserInventoryById_NotFoundUser()
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
        public async Task GET_GetWithdrawnHistoriesById_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies();

            // Act 
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/{DependenciesGuids["User"]}/history/withdrawns", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_GetUserInventoryById_OK()
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
        public async Task GET_ActivateUsedPromocode_Conflict()
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
        public async Task GET_ActivateAlreadyUsedPromocode_Conflict()
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
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            UserHistoryPromocode historyPromocode = await Context.UserHistoryPromocodes
                .FirstAsync(f => f.Id == DependenciesGuids["UserHistoryPromocode"]);
            Context.UserHistoryPromocodes.Remove(historyPromocode);
            await Context.SaveChangesAsync();

            await _responseService
                .ResponseGetStatusCode($"/api/user/activate/promocode/{promocode.Name}", AccessToken);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user/exchange/promocode/{promocode.Name}", AccessToken);

            // Assert
            await RemoveDependencies();
            await RemoveUserDependency(DependenciesGuids["User"]);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
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
        public async Task POST_CreatePathBanner_OK()
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
        public async Task POST_CreatePathBanner_Unauthorized()
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
        public async Task DELETE_RemovePathBanner_OK()
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
        public async Task DELETE_RemovePathBanner_Unauthorized()
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

            UserHistoryWithdrawn withdrawn = new()
            {
                Id = DependenciesGuids["UserHistoryWithdrawn"],
                Date = DateTime.UtcNow,
                ItemId = item1.Id,
                UserId = DependenciesGuids["User"]
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
            await Context.UserHistoryWithdrawns.AddAsync(withdrawn);
            await Context.UserHistoryOpenings.AddAsync(opening);
            await Context.UserPathBanners.AddAsync(pathBanner);
            await Context.UserInventories.AddAsync(inventory);

            await Context.SaveChangesAsync();
        }

        private async Task RemoveDependencies()
        {
            UpdateContext();
            GameItem item = await Context.GameItems
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);
            LootBox box = await Context.LootBoxes
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);
            Context.GameItems.Remove(item);
            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
