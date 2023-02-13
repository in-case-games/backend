using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserRolesApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public UserRolesApiTest(WebApplicationFactory<Program> app) {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetRoles_ReturnsOk()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode("/Role/user");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetRolesAll_ReturnsOk()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _response.ResponseGetStatusCode("/Role/all");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task POST_CreateRole_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRole newRole = new() { 
                RoleName = "Test",
            };

            //Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/Role/admin", newRole, AccessToken);

            UserRole? role = await _response.ResponseGet<UserRole>($"/Role/{newRole.RoleName}");

            await _response.ResponseDelete($"/Role/admin/{role!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateRole_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRole newRole = new()
            {
                RoleName = "Test",
            };

            //Act
            await _response.ResponsePostStatusCode("/Role/admin", newRole, AccessToken);

            UserRole? role = await _response.ResponseGet<UserRole>($"/Role/{newRole.RoleName}");
            role!.RoleName = "Test2";

            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                "/Role/admin", role, AccessToken);

            await _response.ResponseDelete($"/Role/admin/{role!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task Delete_DeleteRole_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRole newRole = new()
            {
                RoleName = "Test",
            };

            //Act
            await _response.ResponsePostStatusCode("/Role/admin", newRole, AccessToken);
            UserRole? role = await _response.ResponseGet<UserRole>($"/Role/{newRole.RoleName}");
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/Role/admin/{role!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
