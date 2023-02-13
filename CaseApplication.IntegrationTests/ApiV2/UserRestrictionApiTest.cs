using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserRestrictionApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public UserRestrictionApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetRestriction_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRestriction userRestriction = new() { 
                RestrictionName = "MUTE",
                UserId = User.Id
            };

            // Act
            await _response.ResponsePostStatusCode(
                "/UserRestriction/admin", userRestriction, AccessToken);
            UserRestriction? restriction = await _response.ResponseGet<UserRestriction>(
                $"/UserRestriction/name/{userRestriction.RestrictionName}", AccessToken);
            HttpStatusCode statusCodeGet = restriction is null ? 
                HttpStatusCode.NotFound : HttpStatusCode.OK;
            await _response.ResponseDelete($"/UserRestriction/admin/{restriction!.Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetAllRestriction_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRestriction userRestriction = new()
            {
                RestrictionName = "MUTE",
                UserId = User.Id
            };

            // Act
            await _response.ResponsePostStatusCode(
                "/UserRestriction/admin", userRestriction, AccessToken);
            List<UserRestriction> restrictions = (await _response.ResponseGet<List<UserRestriction>>(
                $"/UserRestriction/{userRestriction.UserId}", AccessToken))!;
            HttpStatusCode statusCodeGet = restrictions.Count == 0 ?
                HttpStatusCode.NotFound : HttpStatusCode.OK;
            await _response.ResponseDelete(
                $"/UserRestriction/admin/{restrictions![0].Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task POST_CreateRestriction_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRestriction userRestriction = new()
            {
                RestrictionName = "MUTE",
                UserId = User.Id
            };

            // Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/UserRestriction/admin", userRestriction, AccessToken);
            UserRestriction? restriction = await _response.ResponseGet<UserRestriction>(
                $"/UserRestriction/name/{userRestriction.RestrictionName}", AccessToken);
            await _response.ResponseDelete($"/UserRestriction/admin/{restriction!.Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateRestriction_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRestriction userRestriction = new()
            {
                RestrictionName = "MUTE",
                UserId = User.Id
            };

            // Act
            await _response.ResponsePostStatusCode(
                "/UserRestriction/admin", userRestriction, AccessToken);
            UserRestriction? restriction = await _response.ResponseGet<UserRestriction>(
                $"/UserRestriction/name/{userRestriction.RestrictionName}", AccessToken);
            restriction!.RestrictionName = "VIP";
            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                "/UserRestriction/admin", restriction, AccessToken);
            await _response.ResponseDelete($"/UserRestriction/admin/{restriction!.Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task Delete_DeleteRestriction_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            UserRestriction userRestriction = new()
            {
                RestrictionName = "MUTE",
                UserId = User.Id
            };

            // Act
            await _response.ResponsePostStatusCode(
                "/UserRestriction/admin", userRestriction, AccessToken);
            UserRestriction? restriction = await _response.ResponseGet<UserRestriction>(
                $"/UserRestriction/name/{userRestriction.RestrictionName}", AccessToken);
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/UserRestriction/admin/{restriction!.Id}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
