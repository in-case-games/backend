using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class CaseInventoryApiTest: IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public CaseInventoryApiTest(WebApplicationFactory<Program> app)
        {
            _response = new ResponseHelper(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetCaseInventory_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();

            await InitializeTestDependencies(gameCaseId, gameItemId, caseInvId);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/CaseInventory/{caseInvId}");

            // Assert
            await RemoveTestDependencies(gameItemId, gameCaseId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetByIds_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();

            await InitializeTestDependencies(gameCaseId, gameItemId, caseInvId);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/CaseInventory/ids/{gameCaseId}&{gameItemId}");

            // Assert
            await RemoveTestDependencies(gameItemId, gameCaseId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetAll_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();

            await InitializeTestDependencies(gameCaseId, gameItemId, caseInvId);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/CaseInventory/all/{caseInvId}");

            // Assert
            await RemoveTestDependencies(gameItemId, gameCaseId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task POST_CreateCaseInventory_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            await InitializeTestUser(userId);

            GameCase gameCase = new()
            {
                Id = gameCaseId,
                GroupCasesName = "GCfoupName",
                GameCaseName = "GCTName",
                GameCaseCost = 400,
                GameCaseImage = "GCATImage",
                RevenuePrecentage = 0.1M
            };

            GameItem gameItem = new()
            {
                Id = gameItemId,
                GameItemCost = 1,
                GameItemName = "CASESEXY",
                GameItemRarity = "XYU",
                GameItemImage = "PICTURE"
            };

            await Context.GameCase.AddAsync(gameCase);
            await Context.GameItem.AddAsync(gameItem);

            await Context.SaveChangesAsync();

            CaseInventoryDto caseInventory = new()
            {
                Id = caseInvId,
                GameCaseId = gameCaseId,
                GameItemId = gameItemId,
                LossChance = 10,
                NumberItemsCase = 1
            };

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePostStatusCode($"/CaseInventory/admin", caseInventory, AccessToken);

            // Assert
            await RemoveTestDependencies(gameItemId, gameCaseId);
            await RemoveTestUser(userId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task PUT_UpdateCaseInventory_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            await InitializeTestUser(userId);
            await InitializeTestDependencies(gameCaseId, gameItemId, caseInvId);

            CaseInventoryDto caseInventory = new()
            {
                Id = caseInvId,
                GameCaseId = gameCaseId,
                GameItemId = gameItemId,
                LossChance = 10,
                NumberItemsCase = 1
            };

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut($"/CaseInventory/admin", caseInventory, AccessToken);

            // Assert
            await RemoveTestDependencies(gameItemId, gameCaseId);
            await RemoveTestUser(userId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task DELETE_DeleteCaseInventory_ReturnsOk()
        {
            // Arrange
            Guid gameCaseId = Guid.NewGuid();
            Guid gameItemId = Guid.NewGuid();
            Guid caseInvId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            await InitializeTestUser(userId);

            await InitializeTestDependencies(gameCaseId, gameItemId, caseInvId);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/CaseInventory/admin/{caseInvId}", AccessToken);

            await _response.ResponseDelete(
                $"/GameItem/admin/{gameItemId}", AccessToken);
            await _response
                .ResponseDelete($"/GameCase/admin/{gameCaseId}", AccessToken);
            // Assert

            await RemoveTestUser(userId);
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        #region Начальные данные

        private async Task InitializeTestDependencies(Guid gameCaseId, Guid gameItemId, Guid caseInvId)
        {
            GameCase gameCase = new()
            {
                Id = gameCaseId,
                GroupCasesName = "GCfoupName",
                GameCaseName = "GCTName",
                GameCaseCost = 400,
                GameCaseImage = "GCATImage",
                RevenuePrecentage = 0.1M
            };

            GameItem? gameItem = new()
            {
                Id = gameItemId,
                GameItemCost = 1,
                GameItemName = "CASESEXY",
                GameItemRarity = "XYU",
                GameItemImage = "PICTURE"
            };

            CaseInventory? caseInventory = new()
            {
                Id = caseInvId,
                GameCaseId = gameCaseId,
                GameCase = gameCase,
                GameItem = gameItem,
                GameItemId = gameItemId,
                LossChance = 10,
                NumberItemsCase = 1
            };

            await Context.GameCase.AddAsync(gameCase);
            await Context.GameItem.AddAsync(gameItem);
            await Context.CaseInventory.AddAsync(caseInventory);

            await Context.SaveChangesAsync();
        }

        private async Task RemoveTestDependencies(Guid gameItemId, Guid gameCaseId)
        {
            GameItem? gameItem = await Context.GameItem.FirstOrDefaultAsync(x => x.Id == gameItemId);
            Context.GameItem.Remove(gameItem!);

            GameCase? gameCase = await Context.GameCase.FirstOrDefaultAsync(x => x.Id == gameCaseId);
            Context.GameCase.Remove(gameCase!);

            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
