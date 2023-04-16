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
        [Fact]
        public async Task GET_GetUserAfterAuthorize_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid, "admin");

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
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
        public async Task GET_GetEmptyUserPromocodeHistories_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user/history/promocodes");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetEmptyUserWithdrawnHistories_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user/history/withdrawns");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
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
                .ResponseGetStatusCode($"/api/user/{guid}/history/withdrawns");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
            await RemoveUserDependency(guid);
        }
        [Fact]
        public async Task GET_GetEmptyUserHistoryOpenings_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeUserDependency(guid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user/history/openings");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
            await RemoveUserDependency(guid); 
        }
    }
}
