using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRolesApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserRolesApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private string GenerateString()
        {
            byte[] bytes = new byte[2];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes).Replace('=', 's');
        }
        [Fact]
        public async Task UserRoleCrudTest()
        {
            // Arrange
            UserRole templateUserRole = new UserRole()
            { 
                RoleName = GenerateString()
            };

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost<UserRole>("/Role", templateUserRole);

            UserRole userRole = await _response
                .ResponseGet<UserRole>($"/Role?roleName={templateUserRole.RoleName}");

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/Role?roleName={templateUserRole.RoleName}");
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode("/Role/GetAllRoles");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut<UserRole>("/Role", userRole);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/Role?id={userRole.Id}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));
        }
    }
}
