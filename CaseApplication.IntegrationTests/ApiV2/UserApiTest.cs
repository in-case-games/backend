using CaseApplication.Api.Controllers;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _accessToken;
        private readonly AuthenticationHelper _helper = new();
        private readonly ResponseHelper _response;
        private static readonly Guid guid = Guid.NewGuid();
        public UserApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
            _accessToken = _helper.CreateToken();
        }
        [Fact]
        public async Task GET_AllUsers_ShouldBeOk()
        {
            // Arrange
            throw new Exception(_accessToken);
            // Act

            // Assert
        }
        [Fact]
        public async Task GET_GetById_ShouldBeOk()
        {
            // Arrange

            // Act

            // Assert
        }
        [Fact]
        public async Task GET_GetByNonExistedId_ShouldBeNotFound()
        {
            // Arrange

            // Act

            // Assert
        }
        [Fact]
        public async Task GET_GetByLogin_ShouldBeOk()
        {
            // Arrange

            // Act

            // Assert
        }
        [Fact]
        public async Task GET_GetByNonExistedLogin_ShouldBeNotFound()
        {
            // Arrange

            // Act

            // Assert
        }
        private static List<User> InitializeUserLibrary()
        {
            var users = new List<User>();

            UserAdditionalInfo info = new UserAdditionalInfo
            {
                UserRole = new UserRole { RoleName = "user" }
            };
            users.Add(new User
            {
                Id = guid,
                UserLogin = "UserUserForTests1",
                UserEmail = "UserEmailUserForTest1",
                UserAdditionalInfo = info
            });


            return users;
        }
    }
}
