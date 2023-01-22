using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserApiTest(WebApplicationFactory<Program> application)
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
        public async Task GetAllUsersTest()
        {
            // Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode("/User/GetAll");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetUserTest()
        {
            // Arrange
            User user = InitializeUser();

            // Act
            await _response.ResponsePost<User>("/User", user);
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/User?email={user.UserEmail}&hash=123");
            await DeleteUser(user.UserEmail!);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetImpossibleUserTest()
        {
            // Arrange
            string email = "' SELECT * FROM User --%20";
            string hash = "' DROP DATABASE;";

            // Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode(
                $"/User?email={email}&hash={hash}");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        }
        [Fact]
        public async Task PostUserTest()
        {
            // Arrange
            User user = InitializeUser();

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost("/User", user);
            await DeleteUser(user.UserEmail!);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PostEmptyUserTest()
        {
            // Arrange
            User user = new User();

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost("/User", user);

            // Assert
            Assert.True(statusCode != HttpStatusCode.OK);
        }
        [Fact]
        public async Task PutUserTest()
        {
            // Arrange
            User oldUser = InitializeUser();
            User newUser = oldUser;
            newUser.UserName = oldUser.UserName + newUser.UserName;

            // Act
            await _response.ResponsePost("/User", oldUser);

            IEnumerable<User> users = await _response.ResponseGet<List<User>>($"/User/GetAll");
            newUser.Id = users.FirstOrDefault(x => x.UserEmail == oldUser.UserEmail)!.Id;

            HttpStatusCode statusCode = await _response.ResponsePut("/User?hash=123", newUser);
            await DeleteUser(newUser.UserEmail!);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutNotExistedUserTest()
        {
            // Arrange
            User user = new User();

            // Act
            HttpStatusCode statusCode = await _response.ResponsePut<User>("/User?hash=123", user);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteUserTest()
        {
            // Arrange
            User user = InitializeUser();
            await _response.ResponsePost("/User", user);

            IEnumerable<User> users = await _response.ResponseGet<List<User>>($"/User/GetAll");
            user.Id = users.FirstOrDefault(x => x.UserEmail == user.UserEmail)!.Id;
            // Act
            HttpStatusCode statusCode = await _response.ResponseDelete($"/User?id={user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteNotExistedUserTest()
        {
            // Arrange
            User user = new User();

            // Act
            HttpStatusCode statusCode = await _response.ResponseDelete($"/User?id={user.Id}");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
    }
}
