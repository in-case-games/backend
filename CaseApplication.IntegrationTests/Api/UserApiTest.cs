using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private AuthenticationTestHelper _authHelper;
        private TokenModel AdminToken { get; set; } = new();
        private User User { get; set; } = new();
        private TokenModel UserToken { get; set; } = new();
        private User Admin { get; set; } = new();
        public UserApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
            _authHelper = new AuthenticationTestHelper(_response);
        }
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULAT{ipUser}User",
                UserEmail = $"UEAT{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULAT{ipAdmin}Admin",
                UserEmail = $"UEAT{ipAdmin}Admin"
            };
            UserToken = await _authHelper.SignInUser(User, ipUser);
            AdminToken = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }
        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULUIAT{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"UEUIAT{ipAdmin}Admin");
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = "testuser1",
                UserEmail = "testuser1",
                UserImage = "testuser1",
                PasswordHash = "testuser1",
                PasswordSalt = "testuser1",
            };

            return user;
        }
        [Fact]
        public async Task UserCrudTest()
        {
            // Arrange
            await InitializeOneTimeAccounts("0.10.0", "0.10.1");
            User templateUser = InitializeUser();

            // Act
            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/User/GetByLogin?login={User.UserEmail}",
                token: UserToken.AccessToken!);
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode("/User/GetAll", token: UserToken.AccessToken!);

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/User/UpdateLogin?login=asdas",
                User,
                UserToken.AccessToken!);

            await DeleteOneTimeAccounts("0.10.0", "0.10.1");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (getStatusCode, getAllStatusCode, putStatusCode));
        }
    }
}
