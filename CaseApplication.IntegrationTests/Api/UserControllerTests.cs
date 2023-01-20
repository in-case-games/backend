using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Repositories;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserControllerTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ClientApiRepository _clientApi;
        public UserControllerTests(WebApplicationFactory<Program> application)
        {
            _clientApi = new(application.CreateClient());
            _client = application.CreateClient();
        }
        private async Task DeleteUser(params string[] emails)
        {
            foreach (string email in emails)
            {
                List<User> users = await _clientApi.CreateResponseGet<List<User>>("/User/GetAll");
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
            HttpResponseMessage response = await _client.GetAsync("/User/GetAll");

            //Assert
            Assert.True(response.IsSuccessStatusCode);
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
            PostEntityModel<User> postEntityModel = new PostEntityModel<User>
            {
                PostUrl = "/User",
                PostContent = user
            };

            // Act
            HttpStatusCode code = await _clientApi.CreateResponsePost(postEntityModel);
            await DeleteUser(user.UserEmail);

            // Assert
            Assert.True(code == HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetImpossibleUserTest()
        {
            // Arrange
            string email = "' SELECT * FROM User --%20";
            string hash = "' DROP DATABASE;";

            // Act
            HttpResponseMessage response = await _client.GetAsync($"/User?email={email}&hash={hash}");

            // Assert
            Assert.True(!response.IsSuccessStatusCode);
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
            PostEntityModel<User> postEntityModel = new PostEntityModel<User>
            {
                PostUrl = "/User",
                PostContent = user
            };
            HttpStatusCode code = await _clientApi.CreateResponsePost(postEntityModel);
            await DeleteUser(user.UserEmail);

            // Assert
            Assert.True(code == HttpStatusCode.OK);
        }
        [Fact]
        public async Task PostEmptyUserTest()
        {
            // Arrange
            User user = new User();

            // Act
            PostEntityModel<User> postEntityModel = new PostEntityModel<User>
            {
                PostUrl = "/User",
                PostContent = user
            };
            HttpStatusCode code = await _clientApi.CreateResponsePost(postEntityModel);

            //Assert
            Assert.True(code != HttpStatusCode.OK);
        }
        
    }
}
