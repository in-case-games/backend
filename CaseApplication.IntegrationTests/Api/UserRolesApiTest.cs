using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualBasic;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRolesApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private AuthenticationTestHelper _authHelper;
        public UserRolesApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
            _authHelper = new AuthenticationTestHelper(_response);
        }
        private string GenerateString()
        {
            byte[] bytes = new byte[2];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes).Replace('=', 's').Replace('+', '1');
        }
        [Fact]
        public async Task UserRoleCrudTest()
        {
            User admin = new User()
            {
                UserLogin = $"ULUIAT0.14.0Admin",
                UserEmail = $"UEUIAT0.14.0Admin"
            }; ;
            TokenModel adminToken = await _authHelper.SignInAdmin(admin, "0.14.0");
            // Arrange
            UserRole templateUserRole = new UserRole()
                { 
                RoleName = GenerateString()
            };

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePostStatusCode("/Role", templateUserRole, token: adminToken.AccessToken!);

            UserRole? userRole = await _response
                .ResponseGet<UserRole>($"/Role?name={templateUserRole.RoleName}",
                token: adminToken.AccessToken!);

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/Role?name={templateUserRole.RoleName}",
                token: adminToken.AccessToken!);
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode("/Role/GetAll", token: adminToken.AccessToken!);

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/Role", userRole!, token: adminToken.AccessToken!);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/Role?id={userRole!.Id}", token: adminToken.AccessToken!);
            await _authHelper.DeleteUserByAdmin($"ULUIAT0.14.0Admin");
            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));
        }
    }
}
