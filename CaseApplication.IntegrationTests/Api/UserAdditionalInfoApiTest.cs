using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserAdditionalInfoApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private AuthenticationTestHelper _authHelper;
        private TokenModel AdminToken { get; set; } = new();
        private User User { get; set; } = new();
        private TokenModel UserToken { get; set; } = new();
        private User Admin { get; set; } = new();
        public UserAdditionalInfoApiTest(WebApplicationFactory<Program> application)
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
            await _authHelper.DeleteUserByAdmin($"ULUIAT{ipUser}User", UserToken.AccessToken!);
            await _authHelper.DeleteUserByAdmin($"UEUIAT{ipAdmin}Admin", AdminToken.AccessToken!);
        }
        private async Task<UserAdditionalInfo> CreateUserAdditionalInfo()
        {
            User? currentUser = await _response
                .ResponseGet<User>($"/User/GetByLogin?login={User.UserLogin}");
            UserAdditionalInfo userInfo = new UserAdditionalInfo()
            {
                UserId = currentUser!.Id,
                UserBalance = 99,
                UserAbleToPay = 0
            };

            return userInfo;
        }
        [Fact]
        public async Task UserAdditionalInfoCrudTest()
        {
            // Arrange
            await InitializeOneTimeAccounts("0.9.0", "0.9.1");
            UserAdditionalInfo templateUserInfo = await CreateUserAdditionalInfo();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePostStatusCode("/UserAdditionalInfo",
                templateUserInfo,
                UserToken.AccessToken!);

            UserAdditionalInfo? userInfo = await _response
                .ResponseGet<UserAdditionalInfo>($"/UserAdditionalInfo",
                UserToken.AccessToken!);
            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserAdditionalInfo");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/UserAdditionalInfo/UpdateInfoByAdmin", userInfo!, AdminToken.AccessToken!);



            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, putStatusCode));

            await DeleteOneTimeAccounts("0.9.0", "0.9.1");
        }
      
    }
}
