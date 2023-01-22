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
        public async Task GetNotExistRoleTest()
        {
            // Arrange
            string roleName = "' SELECT * FROM User --%20";

            // Act 
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode($"/Role?roleName={roleName}");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
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
        public async Task PostEmptyRoleTest()
        {
            // Arrange
            UserRole userRole = new UserRole();

            // Act
            HttpStatusCode statusCode = await _response.ResponsePost<UserRole>("/Role", userRole);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
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
        [Fact]
        public async Task PutNotExistRoleTest()
        {
            // Arrange 
            UserRole userRole = new UserRole();

            // Act
            HttpStatusCode statusCode = await _response.ResponsePut<UserRole>("/Role", userRole);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);

        }
        [Fact]
        public async Task DeleteRoleTest()
        {
            // Arrange
            UserRole newUserRole = new UserRole
            {
                RoleName = GenerateString()
            };
            HttpStatusCode postStatusCode = await _response.ResponsePost<UserRole>("/Role", newUserRole);
            UserRole userRole = await _response
                .ResponseGet<UserRole>($"/Role?roleName={newUserRole.RoleName}");
            // Act
            HttpStatusCode statusCode = await _response.ResponseDelete($"/Role?id={userRole.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteNotExistRoleTest()
        {
            // Arrange
            UserRole userRole = new UserRole();

            // Act
            HttpStatusCode statusCode = await _response.ResponseDelete($"/Role?id={userRole.Id}");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
    }
}
