using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class LootBoxGroupTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["LootBox"] = Guid.NewGuid(),
            ["GroupLootBox"] = Guid.NewGuid(),
            ["LootBoxGroup"] = Guid.NewGuid()
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public LootBoxGroupTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Fact]
        public async Task GET_LootBoxGroups_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroups_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroupById_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroupById_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroupByGameId_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/game/{boxGroup.GameId}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroupByGameId_NotFoundGame()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/game/{Guid.NewGuid()}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxGroupByGameId_NotFoundGroup()
        {
            //Arrange
            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();
            Guid gameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/game/{gameId}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_GroupLootBoxes_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/groups");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_GroupLootBoxes_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box-group/groups");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxGroup_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxGroup_NotFoundGame()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            boxGroup.GameId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxGroup_NotFoundGroup()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            boxGroup.GroupId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxGroup_NotFoundBox()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            boxGroup.BoxId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxGroup_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_LootBoxGroup_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBoxGroup boxGroup = await Context.LootBoxGroups
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxGroup"]);

            Context.LootBoxGroups.Remove(boxGroup);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group", boxGroup.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_GroupLootBox_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GroupLootBox groupBox = await Context.GroupLootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GroupLootBox"]);

            Context.GroupLootBoxes.Remove(groupBox);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group/group", groupBox, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_GroupLootBox_Conflict()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            GroupLootBox groupBox = await Context.GroupLootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GroupLootBox"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group/group", groupBox, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Fact]
        public async Task POST_GroupLootBox_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            GroupLootBox groupBox = await Context.GroupLootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GroupLootBox"]);

            Context.GroupLootBoxes.Remove(groupBox);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group/group", groupBox);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_GroupLootBox_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            GroupLootBox groupBox = await Context.GroupLootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["GroupLootBox"]);

            Context.GroupLootBoxes.Remove(groupBox);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box-group/group", groupBox, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxGroup_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxGroup_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxGroup_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task DELETE_LootBoxGroup_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/{DependenciesGuids["LootBoxGroup"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_GroupLootBox_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/group/{DependenciesGuids["GroupLootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_GroupLootBox_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/group/{DependenciesGuids["GroupLootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_GroupLootBox_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/group/{DependenciesGuids["GroupLootBox"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task DELETE_GroupLootBox_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box-group/group/{DependenciesGuids["GroupLootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            UpdateContext();

            List<Domain.Entities.Resources.Game> games = await Context.Games.ToListAsync();

            Guid csgoGameId = games.FirstOrDefault(f => f.Name == "csgo")!.Id;

            GroupLootBox groupBox = new()
            {
                Id = DependenciesGuids["GroupLootBox"],
                Name = GenerateString(8)
            };

            LootBox box = new()
            {
                Id = DependenciesGuids["LootBox"],
                Name = "GCNGOCATName",
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxGroup boxGroup = new()
            {
                Id = DependenciesGuids["LootBoxGroup"],
                BoxId = box.Id,
                GameId = csgoGameId,
                GroupId = groupBox.Id
            };

            await Context.LootBoxes.AddAsync(box);
            await Context.GroupLootBoxes.AddAsync(groupBox);
            await Context.LootBoxGroups.AddAsync(boxGroup);
            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<GroupLootBox> groupBoxes = await Context.GroupLootBoxes
                .AsNoTracking()
                .ToListAsync();
            List<LootBox> boxes = await Context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            foreach (var group in groupBoxes)
                Context.GroupLootBoxes.Remove(group);
            foreach (var box in boxes)
                Context.LootBoxes.Remove(box);

            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
