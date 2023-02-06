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
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private GameItem GameItem { get; set; } = new();

        private Random random = new();

        public GameItemApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());

            GameItem = new()
            {
                GameItemCost = 100M,
                GameItemImage = "GIIGISTImage",
                GameItemName = $"GIIGISTName",
                GameItemRarity = "Rare"
            };
        }

        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULGIST{ipUser}User",
                UserEmail = $"UEGIST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULGIST{ipAdmin}Admin",
                UserEmail = $"UEGIST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULGIST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULGIST{ipAdmin}Admin");
        }

        [Fact]
        public async Task GameItemSimpleTests()
        {
            await InitializeOneTimeAccounts("0.0.0", "0.0.1");

            //Post
            HttpStatusCode statusCodeCreate = await _clientApi.ResponsePostStatusCode(
                "/GameItem", GameItem, AdminTokens.AccessToken!);

            //Get && Get All
            Guid itemId = await SearchIdItemByName(GameItem.GameItemName!);

            //Put
            GameItem.Id = itemId;
            GameItem.GameItemCost = 120M;

            HttpStatusCode statusCodePut = await _clientApi.ResponsePut(
                "/GameItem", GameItem, AdminTokens.AccessToken!);

            //Delete
            HttpStatusCode statusCodeDelete = await _clientApi.ResponseDelete(
                $"/GameItem?id={itemId}", AdminTokens.AccessToken!);

            bool IsPassedTests = (statusCodeCreate == HttpStatusCode.OK) &&
                (statusCodePut == HttpStatusCode.OK) &&
                (statusCodeDelete == HttpStatusCode.OK);

            await DeleteOneTimeAccounts("0.0.0", "0.0.1");

            Assert.True(IsPassedTests);
        }

        [Fact]
        public async Task GameItemCheckRolesTest()
        {
            await InitializeOneTimeAccounts("0.0.2", "0.0.3");

            HttpStatusCode statusCodeCreate = await _clientApi.ResponsePostStatusCode(
                "/GameItem", GameItem, AdminTokens.AccessToken!);

            Guid itemId = await SearchIdItemByName(GameItem.GameItemName!);

            HttpStatusCode statusCodeDeleteUser = await _clientApi.ResponseDelete(
                $"/GameItem?id={itemId}", UserTokens.AccessToken!);
            HttpStatusCode statusCodeDeleteAdmin = await _clientApi.ResponseDelete(
                $"/GameItem?id={itemId}", AdminTokens.AccessToken!);

            await DeleteOneTimeAccounts("0.0.2", "0.0.3");

            bool IsPassedTests = (statusCodeCreate == HttpStatusCode.OK) &&
                (statusCodeDeleteUser == HttpStatusCode.Forbidden) &&
                (statusCodeDeleteAdmin == HttpStatusCode.OK);

            Assert.True(IsPassedTests);
        }

        private async Task<Guid> SearchIdItemByName(string name)
        {
            GameItem? gameItem = await _clientApi.ResponseGet<GameItem?>(
                $"/GameItem/GetByName?" +
                $"name={name}");

            if (gameItem == null) throw new Exception("No such item");

            return gameItem.Id;
        }
    }
}
