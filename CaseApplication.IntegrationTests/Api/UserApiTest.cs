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
        private string GenerateString()
        {
            byte[] bytes = new byte[10];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes).Replace('=', 's');
        }
        [Fact]
        public async Task UserCrudTest()
        {
            // Arrange
            User templateUser = InitializeUser();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost<User>("/User", templateUser);

            User user = await _response
                .ResponseGet<User>($"/User/GetByLogin?login={templateUser.UserLogin}&hash=123");

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/User/GetByEmail?email={templateUser.UserEmail}&hash=123");
            HttpStatusCode getByLoginStatusCode = await _response
                .ResponseGetStatusCode($"/User/GetByLogin?login={user.UserLogin}&hash=123");
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode("/User/GetAll");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut<User>("/User?hash=123", user);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/User?id={user.Id}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, getByLoginStatusCode, putStatusCode, deleteStatusCode));
        }
    }
}
