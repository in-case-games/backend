using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class LootBoxTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["LootBox"] = Guid.NewGuid(),
            ["LootBoxBanner"] = Guid.NewGuid(),
            ["GameItem"] = Guid.NewGuid(),
            ["LootBoxInventory"] = Guid.NewGuid()
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public LootBoxTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Fact]
        public async Task GET_LootBoxes_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxes_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxById_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxById_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_LootBoxByAdmin_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/admin", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_LootBoxByAdmin_NotFound(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/admin", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxByAdmin_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/admin");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxByAdmin_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/admin", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxInventory_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/inventory");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxInventory_NotFoundBox()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{Guid.NewGuid()}/inventory");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxInventory_NotFoundInventory()
        {
            //Arrange
            await InitializeTestDependencies();

            List<LootBoxInventory> inventories = await Context.LootBoxInventories
                .AsNoTracking()
                .Where(w => w.BoxId == DependenciesGuids["LootBox"])
                .ToListAsync();

            foreach(var inventory in inventories)
                Context.LootBoxInventories.Remove(inventory);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/inventory");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxBanners_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/banners");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxBanners_NotFound()
        {
            //Arrange
            await InitializeTestDependencies();

            List<LootBoxBanner> banners = await Context.LootBoxBanners
                .AsNoTracking()
                .Where(w => w.BoxId == DependenciesGuids["LootBox"])
                .ToListAsync();

            foreach (var banner in banners)
                Context.LootBoxBanners.Remove(banner);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/banners");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxBannerById_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/banner");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxBannerById_NotFoundBox()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/banner");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_LootBoxBannerById_NotFoundBanner()
        {
            //Arrange
            await InitializeTestDependencies();

            List<LootBoxBanner> banners = await Context.LootBoxBanners
                .AsNoTracking()
                .Where(w => w.BoxId == DependenciesGuids["LootBox"])
                .ToListAsync();

            foreach (var banner in banners)
                Context.LootBoxBanners.Remove(banner);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/loot-box/{DependenciesGuids["LootBox"]}/banner");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBox_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box", box.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_LootBox_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();

            box.GameId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box", box.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBox_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box", box.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_LootBox_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            Context.LootBoxes.Remove(box);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box", box.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxInventory_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxInventory inventory = await Context.LootBoxInventories
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxInventory"]);

            Context.LootBoxInventories.Remove(inventory);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/inventory", inventory.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxInventory_NotFoundItem()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxInventory inventory = await Context.LootBoxInventories
               .AsNoTracking()
               .FirstAsync(f => f.Id == DependenciesGuids["LootBoxInventory"]);

            Context.LootBoxInventories.Remove(inventory);
            await Context.SaveChangesAsync();

            inventory.ItemId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/inventory", inventory.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxInventory_NotFoundBox()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxInventory inventory = await Context.LootBoxInventories
               .AsNoTracking()
               .FirstAsync(f => f.Id == DependenciesGuids["LootBoxInventory"]);

            Context.LootBoxInventories.Remove(inventory);
            await Context.SaveChangesAsync();

            inventory.BoxId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/inventory", inventory.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxInventory_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBoxInventory inventory = await Context.LootBoxInventories
               .AsNoTracking()
               .FirstAsync(f => f.Id == DependenciesGuids["LootBoxInventory"]);

            Context.LootBoxInventories.Remove(inventory);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/inventory", inventory.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_LootBoxInventory_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBoxInventory inventory = await Context.LootBoxInventories
               .AsNoTracking()
               .FirstAsync(f => f.Id == DependenciesGuids["LootBoxInventory"]);

            Context.LootBoxInventories.Remove(inventory);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/inventory", inventory.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxBanner_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            Context.LootBoxBanners.Remove(banner);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/banner", banner.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxBanner_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            Context.LootBoxBanners.Remove(banner);
            await Context.SaveChangesAsync();

            banner.BoxId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/banner", banner.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxInventory_Conflict()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/banner", banner.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Fact]
        public async Task POST_LootBoxBanner_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            Context.LootBoxBanners.Remove(banner);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/banner", banner.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_LootBoxBanner_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            Context.LootBoxBanners.Remove(banner);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/loot-box/banner", banner.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task PUT_LootBox_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box", box.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_LootBox_NotFoundGame()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            box.GameId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box", box.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_LootBox_NotFoundBox()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box", box.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_LootBox_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box", box.Convert(false));

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task PUT_LootBox_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box", box.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task PUT_LootBoxBanner_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_LootBoxBanner_NotFoundBox()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            banner.BoxId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_LootBoxBanner_NotFoundBanner()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_LootBoxInventory_Conflict()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);
            LootBox box = await Context.LootBoxes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBox"]);

            box.Id = Guid.NewGuid();
            banner.Id = Guid.NewGuid();
            banner.BoxId = box.Id;

            await Context.LootBoxes.AddAsync(box);
            await Context.LootBoxBanners.AddAsync(banner);
            await Context.SaveChangesAsync();

            banner.Box = null;
            banner.BoxId = DependenciesGuids["LootBox"];

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Fact]
        public async Task PUT_LootBoxBanner_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(false));

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task PUT_LootBoxBanner_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            LootBoxBanner banner = await Context.LootBoxBanners
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["LootBoxBanner"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/loot-box/banner", banner.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBox_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBox_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBox_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/{DependenciesGuids["LootBox"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task Delete_LootBox_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/{DependenciesGuids["LootBox"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxBanner_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/banner/{DependenciesGuids["LootBoxBanner"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxBanner_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/banner/{DependenciesGuids["LootBoxBanner"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxBanner_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/banner/{DependenciesGuids["LootBoxBanner"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task Delete_LootBoxBanner_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/banner/{DependenciesGuids["LootBoxBanner"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxInventory_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/inventory/{DependenciesGuids["LootBoxInventory"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxInventory_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/inventory/{DependenciesGuids["LootBoxInventory"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_LootBoxInventory_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/inventory/{DependenciesGuids["LootBoxInventory"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task Delete_LootBoxInventory_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/loot-box/inventory/{DependenciesGuids["LootBoxInventory"]}", AccessToken);

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

            LootBox box = new()
            {
                Id = DependenciesGuids["LootBox"],
                Name = "GCNGOCATName",
                Cost = 400,
                ImageUri = "GCIGOCATImage",
                GameId = csgoGameId
            };

            LootBoxInventory inventory = new()
            {
                Id = DependenciesGuids["LootBoxInventory"],
                BoxId = box.Id,
                ItemId = item.Id,
                ChanceWining = 1000
            };

            LootBoxBanner banner = new()
            {
                Id = DependenciesGuids["LootBoxBanner"],
                BoxId = box.Id,
                CreationDate = DateTime.UtcNow
            };

            await Context.LootBoxes.AddAsync(box);
            await Context.GameItems.AddAsync(item);
            await Context.LootBoxInventories.AddAsync(inventory);
            await Context.LootBoxBanners.AddAsync(banner);
            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<GameItem> items = await Context.GameItems
                .AsNoTracking()
                .ToListAsync();
            List<LootBox> boxes = await Context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in items)
                Context.GameItems.Remove(item);
            foreach(var box in boxes)
                Context.LootBoxes.Remove(box);

            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
