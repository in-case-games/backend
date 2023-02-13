using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserApiTest: IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public UserApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_AllUsers_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/User/all", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
        [Fact]
        public async Task GET_GetById_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User/{guid}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetByNonExistedId_ReturnsNotFound()
        {
            // Arrange

            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);
            Guid guid2 = Guid.NewGuid();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User/{guid2}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetByLogin_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/User/login/UserUserForTests1", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetByNonExistedLogin_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);
            string login = "not-exist-Br0";

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User/login/{login}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateLogin_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);
            string login = "upd4t3-Br0";
            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut($"/User/login/{login}", new User(), AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task DELETE_DeleteByAdmin_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);
            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/User/admin/{guid}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
    }
}
