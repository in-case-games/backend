using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserAdditionalInfoApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserAdditionalInfoApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task<UserAdditionalInfo> CreateUserAdditionalInfo()
        {
            User user = InitializeUser();
            await _response.ResponsePost<User>("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User?email={user.UserEmail}&hash=123");
            UserAdditionalInfo userInfo = new UserAdditionalInfo()
            {
                UserId = currentUser.Id,
                UserAge = 999,
                UserBalance = 99,
                UserAbleToPay = 9
            };

            return userInfo;
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = GenerateString(),
                UserEmail = GenerateString(),
                UserImage = GenerateString(),
                PasswordHash = GenerateString(),
                PasswordSalt = GenerateString(),
            };

            return user;
        }
        private string GenerateString()
        {
            byte[] bytes = new byte[10];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes).Replace('=', 's');
        }
        [Fact]
        public async Task UserAdditionalInfoCrudTest()
        {
            // Arrange
            UserAdditionalInfo templateUserInfo = await CreateUserAdditionalInfo();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost<UserAdditionalInfo>("/UserAdditionalInfo", templateUserInfo);

            UserAdditionalInfo userInfo = await _response
                .ResponseGet<UserAdditionalInfo>($"/UserAdditionalInfo?userId={templateUserInfo.UserId}");
            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserAdditionalInfo?userId={templateUserInfo.UserId}");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut<UserAdditionalInfo>("/UserAdditionalInfo?hash=123", userInfo);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/UserAdditionalInfo?id={userInfo.Id}");

            await _response.ResponseDelete($"/User?id={templateUserInfo.UserId}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, putStatusCode, deleteStatusCode));
        }
      
    }
}
