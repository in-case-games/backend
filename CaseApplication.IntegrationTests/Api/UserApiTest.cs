using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Repositories;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ResponseHelper _response;
        public UserApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
            _client = application.CreateClient();
        }
        private async Task DeleteUser(params string[] emails)
        {
            foreach (string email in emails)
            {
                IEnumerable<User> users = await _response.ResponseGet<List<User>>("/User/GetAll");
                User? user = users!.FirstOrDefault(x => x.UserEmail == email);

                HttpResponseMessage response = await _client.DeleteAsync($"/User?id={user!.Id}");
            }
            
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
            Assert.True(statusCode == HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetUserTest()
        {
            // Arrange
            User user = new User
            {
                UserName = GenerateString(),
                UserEmail = GenerateString(),
                UserImage = GenerateString(),
                PasswordHash = GenerateString(),
                PasswordSalt = GenerateString(),
            };

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost<User>("/User", user);
            await DeleteUser(user.UserEmail);

            // Assert
            Assert.True(statusCode == HttpStatusCode.OK);
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
            Assert.True(statusCode != HttpStatusCode.OK);
        }
        [Fact]
        public async Task PostUserTest()
        {
            // Arrange
            User user = new User
            {
                UserName = GenerateString(),
                UserEmail = GenerateString(),
                UserImage = GenerateString(),
                PasswordHash = GenerateString(),
                PasswordSalt = GenerateString(),
            };

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost("/User", user);
            await DeleteUser(user.UserEmail);

            // Assert
            Assert.True(statusCode == HttpStatusCode.OK);
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
        
    }
}
