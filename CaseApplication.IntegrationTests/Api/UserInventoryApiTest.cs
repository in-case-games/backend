using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserInventoryApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private AuthenticationTestHelper _authHelper;
        private TokenModel AdminToken { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        public UserInventoryApiTest(WebApplicationFactory<Program> application, AuthenticationTestHelper helper)
        {
            _response = new ResponseHelper(application.CreateClient());
            _authHelper = helper;
        }
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULUIAT{ipUser}User",
                UserEmail = $"UEUIAT{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULUIAT{ipAdmin}Admin",
                UserEmail = $"UEUIAT{ipAdmin}Admin"
            };

            AdminToken = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }
        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"UEUIAT{ipAdmin}Admin");
        }
        private async Task<UserInventory> InitializeUserInventory()
        {
            User? currentUser = await _response
                .ResponseGet<User>($"/User/GetByLogin?login=ULUIAT0.7.1Admin");

            GameItem gameItem = new GameItem()
            {
                GameItemCost = 100M,
                GameItemImage = GenerateString(),
                GameItemName = GenerateString(),
                GameItemRarity = GenerateString()
            };
            await _response.ResponsePostStatusCode("/GameItem", gameItem, token: AdminToken.AccessToken!);
            IEnumerable<GameItem>? gameItems = await _response
                .ResponseGet<List<GameItem>>("/GameItem/GetAll");
            GameItem currentGameItem = gameItems!.FirstOrDefault(x => x.GameItemName == gameItem.GameItemName)!;

            UserInventory userInventory = new UserInventory()
            {
                UserId = currentUser!.Id,
                GameItemId = currentGameItem.Id
            };

            return userInventory;
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = "testinv1",
                UserEmail = "testinv1",
                UserImage = "testinv1",
                PasswordHash = "testinv1",
                PasswordSalt = "testinv1",
            };

            return user;
        }
        private string GenerateString()
        {
            byte[] bytes = new byte[10];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes).Replace('=', 's');
        }
        [Fact]
        public async Task UserInventoryCrudTest()
        {
            // Arrange
            await InitializeOneTimeAccounts("0.12.0", "0.12.1");
            UserInventory templateUserInventory = await InitializeUserInventory();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePostStatusCode("/UserInventory",
                templateUserInventory,
                token: AdminToken.AccessToken!);

            IEnumerable<UserInventory>? userInventories = await _response
                .ResponseGet<List<UserInventory>>
                ($"/UserInventory/GetAll?userId={templateUserInventory.UserId}", token: AdminToken.AccessToken!);
            UserInventory userInventory = userInventories!
                .FirstOrDefault(x =>
                    x.UserId == templateUserInventory.UserId &&
                    x.GameItemId == templateUserInventory.GameItemId)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserInventory?id={userInventory.Id}",
                token: AdminToken.AccessToken!);
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/UserInventory/GetAll?userId={templateUserInventory.UserId}",
                token: AdminToken.AccessToken!);

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/UserInventory", userInventory,
                token: AdminToken.AccessToken!);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/UserInventory?id={userInventory.Id}",
                token: AdminToken.AccessToken!);

            await _response.ResponseDelete($"/User?id={userInventory.UserId}",
                token: AdminToken.AccessToken!);
            await _response.ResponseDelete($"/GameItem?id={userInventory.GameItemId}",
                token: AdminToken.AccessToken!);

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));

            await DeleteOneTimeAccounts("0.12.0", "0.12.1");
        }
    }
}
