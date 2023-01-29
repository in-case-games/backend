using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserInventoryApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserInventoryApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task<UserInventory> InitializeUserInventory()
        {
            User user = InitializeUser();
            await _response.ResponsePost<User>("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User/GetByEmail?email={user.UserEmail}&hash=123");

            GameItem gameItem = new GameItem()
            {
                GameItemCost = 100M,
                GameItemImage = GenerateString(),
                GameItemName = GenerateString(),
                GameItemRarity = GenerateString()
            };
            await _response.ResponsePost<GameItem>("/GameItem", gameItem);
            IEnumerable<GameItem> gameItems = await _response
                .ResponseGet<List<GameItem>>("/GameItem/GetAll");
            GameItem currentGameItem = gameItems.FirstOrDefault(x => x.GameItemName == gameItem.GameItemName)!;

            UserInventory userInventory = new UserInventory()
            {
                UserId = currentUser.Id,
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
            UserInventory templateUserInventory = await InitializeUserInventory();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost<UserInventory>("/UserInventory", templateUserInventory);

            IEnumerable<UserInventory> userInventories = await _response
                .ResponseGet<List<UserInventory>>
                ($"/UserInventory/GetAll?userId={templateUserInventory.UserId}");
            UserInventory userInventory = userInventories
                .FirstOrDefault(x =>
                    x.UserId == templateUserInventory.UserId &&
                    x.GameItemId == templateUserInventory.GameItemId)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserInventory?id={userInventory.Id}");
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/UserInventory/GetAll?userId={templateUserInventory.UserId}");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut<UserInventory>("/UserInventory", userInventory);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/UserInventory?id={userInventory.Id}");

            await _response.ResponseDelete($"/User?id={userInventory.UserId}");
            await _response.ResponseDelete($"/GameItem?id={userInventory.GameItemId}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));
        }
    }
}
