using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class SiteStatisticsTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public SiteStatisticsTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Fact]
        public async Task GET_SiteStatistics_OK()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/site-statistics");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Theory]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_SiteStatisticsAdmin_OK(string role)
        {
            //Arrange
            Guid userGuid = Guid.NewGuid();

            await InitializeUserDependency(userGuid, role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/site-statistics/admin", AccessToken);

            //Assert
            await RemoveUserDependency(userGuid);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_SiteStatisticsAdmin_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/site-statistics/admin");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        public async Task GET_SiteStatisticsAdmin_Forbidden(string role)
        {
            //Arrange
            Guid userGuid = Guid.NewGuid();

            await InitializeUserDependency(userGuid, role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/site-statistics/admin", AccessToken);

            //Assert
            await RemoveUserDependency(userGuid);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        [Fact]
        public async Task PUT_SiteStatisticsAdmin_OK()
        {
            //Arrange
            Guid userGuid = Guid.NewGuid();

            await InitializeUserDependency(userGuid, "bot");

            SiteStatisticsAdmin statistics = await Context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/site-statistics/admin", statistics, AccessToken);

            //Assert
            await RemoveUserDependency(userGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task PUT_SiteStatisticsAdmin_NotFound()
        {
            //Arrange
            Guid userGuid = Guid.NewGuid();

            await InitializeUserDependency(userGuid, "bot");

            SiteStatisticsAdmin statistics = await Context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstAsync();

            statistics.Id = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/site-statistics/admin", statistics, AccessToken);

            //Assert
            await RemoveUserDependency(userGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PUT_SiteStatisticsAdmin_Unauthorized()
        {
            //Arrange
            SiteStatisticsAdmin statistics = await Context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/site-statistics/admin", statistics);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("owner")]
        public async Task PUT_SiteStatisticsAdmin_Forbidden(string role)
        {
            //Arrange
            Guid userGuid = Guid.NewGuid();

            await InitializeUserDependency(userGuid, role);

            SiteStatisticsAdmin statistics = await Context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/site-statistics/admin", statistics, AccessToken);

            //Assert
            await RemoveUserDependency(userGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        #region Начальные данные
        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            SiteStatisticsAdmin statisticsAdmin = await Context.SiteStatisticsAdmins
                .FirstAsync();
            SiteStatistics statistics = await Context.SiteStatistics
                .FirstAsync();

            statisticsAdmin.BalanceWithdrawn = 0;
            statisticsAdmin.TotalReplenished = 0;
            statisticsAdmin.SentSites = 0;

            statistics.LootBoxes = 0;
            statistics.Reviews = 0;
            statistics.WithdrawnFunds = 0;
            statistics.WithdrawnItems = 0;

            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
