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
            await _response.ResponsePost<UserAdditionalInfo>("/UserAdditionalInfo", userInfo);
            UserAdditionalInfo currentUserInfo = await _response.
                ResponseGet<UserAdditionalInfo>($"/UserAdditionalInfo?userId={currentUser.Id}");

            return currentUserInfo;
        }
        private async Task DeleteUser(params Guid[] userIds)
        {
            foreach (Guid userId in userIds)
            {
                await _response.ResponseDelete($"/User?id={userId}");
            }

        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserName = GenerateString(),
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

            return Convert.ToBase64String(bytes);
        }
        [Fact]
        public async Task GetUserAdditionalInfo()
        {
            // Arrange
            UserAdditionalInfo userInfo = await CreateUserAdditionalInfo();
            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/UserAdditionalInfo?userId={userInfo.UserId}");
            await DeleteUser(userInfo!.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetNotExistedUserAdditionalInfo()
        {
            // Arrange 

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/UserAdditionalInfo?userId=00000000-0000-0000-0000-000000000000");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PostUserAdditionalInfo()
        {
            // Arrange
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

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePost<UserAdditionalInfo>("/UserAdditionalInfo", userInfo);
            await DeleteUser(userInfo.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PostEmptyUserAdditionalInfo()
        {
            // Arrange
            UserAdditionalInfo userInfo = new UserAdditionalInfo();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePost<UserAdditionalInfo>("/UserAdditionalInfo", userInfo);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutUserAdditionalInfo()
        {
            // Arrange

            UserAdditionalInfo userInfo = await CreateUserAdditionalInfo();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut<UserAdditionalInfo>("/UserAdditionalInfo?hash=123", userInfo);
            await DeleteUser(userInfo.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutEmptyUserAdditionalInfo()
        {
            // Arrange
            UserAdditionalInfo userInfo = new UserAdditionalInfo();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut<UserAdditionalInfo>("/UserAdditionalInfo?hash=123", userInfo);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteUserAdditionalInfo()
        {
            // Arrange
            UserAdditionalInfo userInfo = await CreateUserAdditionalInfo();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/UserAdditionalInfo?id={userInfo.Id}");
            await DeleteUser(userInfo.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteNotExistUserAdditionalInfo()
        {
            // Arrange
            UserAdditionalInfo userInfo = new UserAdditionalInfo();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/UserAdditionalInfo?id={userInfo.Id}");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
    }
}
