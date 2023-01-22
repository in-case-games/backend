using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameItemApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private Random random = new();

        public GameItemApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task GameItemCreateTest()
        {
            GameItem gameItem = new()
            {
                GameItemCost = 100,
                GameItemImage = "image",
                GameItemName = $"Оружие {random.Next(1, 100000)}",
                GameItemRarity = "Rare"
            };
            HttpStatusCode httpStatusCode = await _clientApi.ResponsePost<GameItem>("/GameItem", gameItem);
            
            Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        }

        [Fact]
        public async Task GameItemGetTest()
        {
            Guid id = await SearchIdLastGameItem();

            GameItem gameItem = await _clientApi.ResponseGet<GameItem>($"/GameItem?id={id}");

            Assert.Equal(id, gameItem.Id);
        }

        [Fact]
        public async Task GameItemGetAllTest()
        {
            List<GameItem> gameItems = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAllItems");

            Assert.True(gameItems.Count > 0);
        }

        [Fact]
        public async Task GameItemUpdateTest()
        {
            Guid id = await SearchIdLastGameItem();

            GameItem gameItem = new()
            {
                Id = id,
                GameItemImage = "image2",
                GameItemCost = 2,
                GameItemName = $"Средний айтем {random.Next(1, 100000)}",
                GameItemRarity = "Simple"
            };

            HttpStatusCode statusCode = await _clientApi.ResponsePut<GameItem>("/GameItem", gameItem);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameItemDeleteTest()
        {
            Guid id = await SearchIdLastGameItem();

            HttpStatusCode statusCode = await _clientApi.ResponseDelete($"/GameItem?id={id}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        private async Task<Guid> SearchIdLastGameItem()
        {
            List<GameItem> gameItems = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAllItems");
            
            Guid id = gameItems.LastOrDefault()!.Id;
            
            return id;
        } 
    }
}
