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
            await _response.ResponsePost("/User?password=1234", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User/GetByLogin?login={user.UserLogin}&hash=123");
            UserAdditionalInfo userInfo = new UserAdditionalInfo()
            {
                UserId = currentUser.Id,
                UserAge = 18,
                UserBalance = 99,
                UserAbleToPay = 0
            };

            return userInfo;
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = "testlogin1",
                UserEmail = "testemail1",
                UserImage = "testimage1",
                PasswordHash = "test1",
                PasswordSalt = "test1",
            };

            return user;
        }
        [Fact]
        public async Task UserAdditionalInfoCrudTest()
        {
            // Arrange
            UserAdditionalInfo templateUserInfo = await CreateUserAdditionalInfo();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost("/UserAdditionalInfo", templateUserInfo);

            UserAdditionalInfo userInfo = await _response
                .ResponseGet<UserAdditionalInfo>($"/UserAdditionalInfo?userId={templateUserInfo.UserId}");
            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserAdditionalInfo?userId={templateUserInfo.UserId}");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/UserAdditionalInfo?hash=123", userInfo);

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
