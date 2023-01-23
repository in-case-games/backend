using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserAdditionalInfoApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserAdditionalInfoApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task DeleteUser(params string[] emails)
        {
            foreach (string email in emails)
            {
                IEnumerable<User> users = await _response.ResponseGet<List<User>>("/User/GetAll");
                User? user = users!.FirstOrDefault(x => x.UserEmail == email);

                await _response.ResponseDelete($"/User?id={user!.Id}");
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

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/UserAdditionalInfo?userId=82360739-e711-4100-aa52-9e3b6ef5e045");

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
            await DeleteUser(user!.UserEmail!);

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
            UserAdditionalInfo userInfo = new UserAdditionalInfo()
            {
                UserId = new Guid("82360739-e711-4100-aa52-9e3b6ef5e045"),
                UserBalance = new Random().Next(10),
                UserAbleToPay = new Random().Next(10),
                UserAge = new Random().Next(10),
            };

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut<UserAdditionalInfo>("/UserAdditionalInfo?hash=123", userInfo);

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
            UserAdditionalInfo currentUserInfo = await _response.
                ResponseGet<UserAdditionalInfo>($"/UserAdditionalInfo?userId={currentUser.Id}");

            // Act
            HttpStatusCode statusCode = await _response.
                ResponseDelete($"/UserAdditionalInfo?id={currentUserInfo.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
    }
}
