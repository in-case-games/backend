using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserApiTest: IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _accessToken;
        private readonly ResponseHelper _response;
        public UserApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
            _accessToken = CreateToken();
        }
        [Fact]
        public async Task GET_AllUsers_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/User/all", _accessToken);

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
                .ResponseGetStatusCode($"/User/{guid}", _accessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
        [Fact]
        public async Task GET_GetByNonExistedId_ReturnsNotFound()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User/{guid}", _accessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_GetByLogin_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/User/login/UserUserForTests1", _accessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
        [Fact]
        public async Task GET_GetByNonExistedLogin_ReturnsNotFound()
        {
            // Arrange
            string login = "not-exist-Br0";

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User/login/{login}", _accessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        #region Начальные данные
        private async Task InitializeTestUser(Guid guid)
        {
            User user = new User
            {
                Id = guid,
                UserLogin = "UserUserForTests1",
                UserEmail = "UserEmailUserForTest1",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1"
            };

            await Context.User.AddAsync(user);
            await Context.SaveChangesAsync();
        }
        private async Task RemoveTestUser(Guid guid)
        {
            User? user = await Context.User.FindAsync(guid);
            Context.User.Remove(user!);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
