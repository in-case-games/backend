using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameItemApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel? UserTokens {get; set;}
        private TokenModel? AdminTokens { get; set;}
        private User? User { get; set; }
        private User? Admin { get; set; }

        private Random random = new();

        public GameItemApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        private async Task InitializeAccounts()
        {
            User = new()
            {
                UserLogin = "ULGIST101User",
                UserEmail = "UEGIST101User"
            };
            Admin = new()
            {
                UserLogin = "ULGIST105Admin",
                UserEmail = "UEGIST105Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, "101");
            AdminTokens = await _authHelper.SignInAdmin(Admin, "105");
        }

        [Fact]
        public async Task GameItemSimpleTests()
        {
            await InitializeAccounts();

            //Post
            GameItem gameItem = new()
            {
                GameItemCost = 100M,
                GameItemImage = "image200",
                GameItemName = $"Оружие {random.Next(1, 100000)}",
                GameItemRarity = "Rare"
            };

            HttpStatusCode statusCodeCreate = await _clientApi
                .ResponsePost<GameItem>("/GameItem", gameItem);

            //Get && Get All
            Guid itemId = await SearchIdItemByName(gameItem.GameItemName);

            //Put
            gameItem.Id = itemId;
            gameItem.GameItemImage = "img231231231";

            HttpStatusCode statusCodePut = await _clientApi.ResponsePut<GameItem>("/GameItem", gameItem);

            //Delete
            HttpStatusCode statusCodeDelete = await _clientApi.ResponseDelete($"/GameItem?id={itemId}");
            
            bool IsPassedTests = (statusCodeCreate == HttpStatusCode.OK) &&
                (statusCodePut == HttpStatusCode.OK) &&
                (statusCodeDelete == HttpStatusCode.OK);

            Assert.True(IsPassedTests);
        }

        private async Task<Guid> SearchIdItemByName(string name)
        {
            List<GameItem> gameItems = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAll");

            GameItem gameItem = gameItems.FirstOrDefault(x => x.GameItemName == name) ?? new();

            return gameItem.Id;
        }
    }
}
