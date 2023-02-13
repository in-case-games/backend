using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class GameCaseApiTest: IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public GameCaseApiTest(WebApplicationFactory<Program> app)
        {
            _response = new ResponseHelper(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetGameCaseById_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestDependencies(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/GameCase/{guid}");

            // Assert
            await RemoveTestDependencies(guid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetGameCaseByName_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestDependencies(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/GameCase/name/GCNGOCAsdfTName");

            // Assert
            await RemoveTestDependencies(guid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetAllGameCases_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestDependencies(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/GameCase/all");

            // Assert
            await RemoveTestDependencies(guid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetAllGameCasesByGroupName_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestDependencies(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/GameCase/groupname/GCNGOCATsdfGroupName");

            // Assert
            await RemoveTestDependencies(guid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetGameCaseByAdmin_ReturnsOk()
        {
            // Arrange
            Guid caseGuid = Guid.NewGuid();
            Guid userGuid = Guid.NewGuid();
            await InitializeTestDependencies(caseGuid);
            await InitializeTestUser(userGuid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/GameCase/admin/{caseGuid}", AccessToken);

            // Assert
            await RemoveTestDependencies(caseGuid);
            await RemoveTestUser(userGuid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task POST_CreateGameCase_ReturnsOk()
        {
            // Arrange
            Guid userGuid = Guid.NewGuid();
            Guid caseGuid = Guid.NewGuid();
            await InitializeTestUser(userGuid);

            GameCaseDto gameCase = new()
            {
                Id = caseGuid,
                GroupCasesName = "GCNGOCATsdfGroupName",
                GameCaseName = "GCNGOCATsdfName",
                GameCaseCost = 400,
                GameCaseImage = "GCIGOCATImage",
                RevenuePrecentage = 0.1M
            };

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePostStatusCode("/GameCase/admin", gameCase, AccessToken);

            GameCase? createdCase = await _response
                .ResponseGet<GameCase>($"/GameCase/name/{gameCase.GameCaseName}", AccessToken);

            // Assert
            await RemoveTestDependencies(createdCase!.Id);
            await RemoveTestUser(userGuid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task PUT_UpdateGameCase_ReturnsOk()
        {
            // Arrange
            Guid userGuid = Guid.NewGuid();
            Guid caseGuid = Guid.NewGuid();
            await InitializeTestUser(userGuid);
            await InitializeTestDependencies(caseGuid);

            GameCaseDto gameCase = new()
            {
                Id = caseGuid,
                GroupCasesName = "GCNGOCATsdfGroupName",
                GameCaseName = "GCNGOCATsdfName",
                GameCaseCost = 400,
                GameCaseImage = "GCIGOCATImage",
                RevenuePrecentage = 0.1M
            };

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut("/GameCase/admin", gameCase, AccessToken);

            // Assert
            await RemoveTestDependencies(caseGuid);
            await RemoveTestUser(userGuid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task DELETE_DeleteGameCase_ReturnsOk()
        {
            // Arrange
            Guid userGuid = Guid.NewGuid();
            Guid caseGuid = Guid.NewGuid();
            await InitializeTestUser(userGuid);
            await InitializeTestDependencies(caseGuid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/GameCase/admin/{caseGuid}", AccessToken);

            // Assert
            await RemoveTestUser(userGuid);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        #region Начальные данные
        private async Task InitializeTestDependencies(Guid guid)
        {
            GameCase gameCase = new()
            {
                Id = guid,
                GroupCasesName = "GCNGOCATGrdsfoupName",
                GameCaseName = "GCNGOCAsdfTName",
                GameCaseCost = 400,
                GameCaseImage = "GCIGOCATImage",
                RevenuePrecentage = 0.1M
            };

            await Context.GameCase.AddAsync(gameCase);
            await Context.SaveChangesAsync();
        }

        private async Task RemoveTestDependencies(Guid guid)
        {
            GameCase? gameCase = await Context.GameCase.FindAsync(guid);

            Context.GameCase.Remove(gameCase!);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
