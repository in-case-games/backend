using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class GameItemApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public GameItemApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new()
            {
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            await _response.ResponsePostStatusCode("/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            HttpStatusCode statusCodeGet = await _response.ResponseGetStatusCode($"/GameItem/{gameItem!.Id}");
            await _response.ResponseDelete($"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetByNameItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new() { 
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            await _response.ResponsePostStatusCode("/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            HttpStatusCode statusCodeGet = gameItem is null ? 
                HttpStatusCode.NotFound : HttpStatusCode.OK;
            await _response.ResponseDelete($"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetAllItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new()
            {
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            await _response.ResponsePostStatusCode("/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            HttpStatusCode statusCodeGet = await _response.ResponseGetStatusCode($"/GameItem/all");
            await _response.ResponseDelete($"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task POST_CreateItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new()
            {
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            await _response.ResponseDelete($"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new()
            {
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            gameItem!.GameItemRarity = "XTI";
            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                $"/GameItem/admin", gameItem, AccessToken);
            await _response.ResponseDelete($"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task DELETE_DeleteItem_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            GameItem? gameItem = new()
            {
                GameItemCost = 1,
                GameItemName = "CASEXY",
                GameItemRarity = "XYT",
                GameItemImage = "PICTURE"
            };

            //Act
            await _response.ResponsePostStatusCode("/GameItem/admin", gameItem, AccessToken);
            gameItem = await _response.ResponseGet<GameItem>($"/GameItem/name/{gameItem.GameItemName}");
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/GameItem/admin/{gameItem!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
