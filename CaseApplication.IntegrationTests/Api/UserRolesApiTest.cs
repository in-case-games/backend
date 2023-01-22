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
            byte[] bytes = new byte[10];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
        [Fact]
        public async Task GetAllRolesTest()
        {
            // Arrange

            // Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode("/Role/GetAllRoles");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetRoleTest()
        {
            // Arrange

            // Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode("/Role?roleName=gettest");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PostRoleTest()
        {
            // Arrange
            UserRole userRole = new UserRole
            {
                RoleName = GenerateString()
            };

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost<UserRole>("/Role", userRole);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutRoleTest()
        {
            // Arrange
            UserRole userRole = new UserRole
            {
                Id = new Guid("622f6996-5def-410c-9247-08dafc600a5b"),
                RoleName = "puttest" + GenerateString()
            };
            // Act
            HttpStatusCode statusCode = await _response.ResponsePut<UserRole>("/Role", userRole);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
    }
}
