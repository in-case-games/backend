using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class PromocodeTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["Promocode"] = Guid.NewGuid()
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public PromocodeTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_Promocode_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/promocode", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_Promocode_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/promocode", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_Promocode_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/promocode");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task GET_Promocode_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/promocode", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_PromocodeByName_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/name/{promocode.Name}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_PromocodeByName_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/name/{promocode.Name}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_PromocodeByName_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/name/{promocode.Name}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_PromocodeById_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/{DependenciesGuids["Promocode"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_PromocodeById_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/{DependenciesGuids["Promocode"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_PromocodeById_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/{DependenciesGuids["Promocode"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_PromocodeTypes_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/types", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_PromocodeTypes_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/promocode/types");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task POST_Promocode_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_Promocode_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            promocode.Type = null;
            promocode.TypeId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_Promocode_ConflictNameAlreadyUsed()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Fact]
        public async Task POST_Promocode_ConflictDiscount()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            promocode.Discount = -1M;

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            promocode.Discount = 1M;

            HttpStatusCode statusCode2 = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.Conflict, HttpStatusCode.Conflict), 
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task POST_Promocode_ConflictNumberActivations()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            promocode.NumberActivations = -1;

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task POST_Promocode_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_Promocode_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            Context.Promocodes.Remove(promocode);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode("/api/promocode", promocode.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_NotFoundPromocode()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_NotFoundType()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            promocode.TypeId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_ConflictNameAlreadyUsed()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            promocode.Id = Guid.NewGuid();
            promocode.Name = GenerateString(8);

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            promocode.Id = DependenciesGuids["Promocode"];

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_ConflictDiscount()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            promocode.Discount = -1M;

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            promocode.Discount = 1M;

            HttpStatusCode statusCode2 = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.Conflict, HttpStatusCode.Conflict),
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task PUT_Promocode_ConflictNumberActivations()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            promocode.NumberActivations = -1;

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Conflict, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task PUT_Promocode_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task PUT_Promocode_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            Promocode promocode = await Context.Promocodes
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["Promocode"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut("/api/promocode", promocode.Convert(false));

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task DELETE_Promocode_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/promocode/{DependenciesGuids["Promocode"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_Promocode_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/promocode/{DependenciesGuids["Promocode"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_Promocode_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/promocode/{DependenciesGuids["Promocode"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task DELETE_Promocode_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/promocode/{DependenciesGuids["Promocode"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            UpdateContext();

            PromocodeType type = await Context.PromocodeTypes
                .AsNoTracking()
                .FirstAsync(f => f.Name == "case");

            Promocode promocode = new()
            {
                Id = DependenciesGuids["Promocode"],
                Discount = 0.1M,
                NumberActivations = 1,
                ExpirationDate = DateTime.UtcNow,
                Name = GenerateString(8),
                TypeId = type.Id,
            };

            await Context.Promocodes.AddAsync(promocode);
            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<Promocode> promocodes = await Context.Promocodes
                .AsNoTracking()
                .ToListAsync();

            foreach (var promocode in promocodes)
                Context.Promocodes.Remove(promocode);

            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
