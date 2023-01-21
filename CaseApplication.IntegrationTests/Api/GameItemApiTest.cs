using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameItemApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ClientApiRepository _clientApi;
        private Random random = new();
        public GameItemApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task GameItemCreateTest()
        {
            PostEntityModel<GameItem> entityModel = new() { 
                PostUrl = "/GameItem",
                PostContent = new GameItem { 
                    GameItemCost = 1,
                    GameItemImage = "image",
                    GameItemName = $"Лучший айтем {random.Next(1, 100000)}",
                    GameItemRarity = "Rare"
                }
            };
            HttpStatusCode httpStatusCode = await _clientApi.CreateResponsePost<GameItem>(entityModel);
            
            Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        }

        [Fact]
        public async Task GameItemGetTest()
        {
            Guid id = await SearchIdLastGameItem();

            GameItem gameItem = await _clientApi.CreateResponseGet<GameItem>($"/GameItem?id={id}");

            Assert.Equal(id, gameItem.Id);
        }

        [Fact]
        public async Task GameItemGetAllTest()
        {
            List<GameItem> gameItems = await _clientApi.CreateResponseGet<List<GameItem>>("/GameItem/GetAllItems");

            Assert.True(gameItems.Count > 0);
        }

        [Fact]
        public async Task GameItemUpdateTest()
        {
            Guid id = await SearchIdLastGameItem();

            PostEntityModel<GameItem> entityModel = new()
            {
                PostUrl = "/GameItem",
                PostContent = new GameItem
                {
                    Id = id,
                    GameItemImage = "image2",
                    GameItemCost = 2,
                    GameItemName = $"Средний айтем {random.Next(1, 100000)}",
                    GameItemRarity = "Simple"
                }
            };

            HttpStatusCode statusCode = await _clientApi.CreateResponsePut<GameItem>(entityModel);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameItemDeleteTest()
        {
            Guid id = await SearchIdLastGameItem();

            HttpStatusCode statusCode = await _clientApi.CreateResponseDelete($"/GameItem?id={id}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        private async Task<Guid> SearchIdLastGameItem()
        {
            List<GameItem> gameItems = await _clientApi.CreateResponseGet<List<GameItem>>("/GameItem/GetAllItems");
            Guid id = gameItems.LastOrDefault()!.Id;
            return id;
        } 
    }
}
