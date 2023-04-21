using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class GameItemTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["GameItem"] = Guid.NewGuid()
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public GameItemTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient(), "https://localhost:7102");
        }

        [Fact]
        public async Task GET_GameItems_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/game-item");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GET_GameItems_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/game-item");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GameItemById_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/game-item/{Guid.NewGuid()}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GET_GameItemById_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/game-item/{DependenciesGuids["GameItem"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Theory]
        [InlineData("/api/game-item/qualities")]
        [InlineData("/api/game-item/types")]
        [InlineData("/api/game-item/rarities")]
        public async Task GET_GameItemDependencies_OK(string uri)
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode(uri);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_NotFoundGame()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.GameId = Guid.NewGuid();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_NotFoundType()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.TypeId = Guid.NewGuid();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_NotFoundRarity()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.RarityId = Guid.NewGuid();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_NotFoundQuality()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.QualityId = Guid.NewGuid();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task POST_GameItem_Unauthorized()
        {
            //Arrange
            GameItem item = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_GameItem_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.Cost = 200M;

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_Unauthorized()
        {
            //Arrange
            GameItem item = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task PUT_GameItem_NotFoundQuality()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.QualityId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_NotFoundType()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.TypeId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_NotFoundRarity()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.RarityId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_NotFoundGame()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            item.GameId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PUT_GameItem_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/game-item", item.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task PUT_GameItem_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            GameItem item = await Context.GameItems
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GameItem"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/game-item", item.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        
        [Fact]
        public async Task Delete_GameItem_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/game-item/{DependenciesGuids["GameItem"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Delete_GameItem_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/game-item/{Guid.NewGuid()}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task Delete_GameItem_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/game-item/{Guid.NewGuid()}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            UpdateContext();

            List<GameItemRarity> rarities = await Context.GameItemRarities.ToListAsync();

            List<GameItemType> types = await Context.GameItemTypes.ToListAsync();

            List<GameItemQuality> qualities = await Context.GameItemQualities.ToListAsync();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            //Create Game Item
            GameItem item = new()
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

            await Context.GameItems.AddAsync(item);
            await Context.SaveChangesAsync();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<GameItem> items = await Context.GameItems
                .AsNoTracking()
                .ToListAsync();

            foreach(var item in items)
            {
                Context.GameItems.Remove(item);
                await Context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
