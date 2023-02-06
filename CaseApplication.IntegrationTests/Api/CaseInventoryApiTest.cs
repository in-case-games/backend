using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class CaseInventoryApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private GameCase GameCase { get; set; } = new();
        private GameItem GameItemFirst { get; set; } = new();
        private GameItem GameItemSecond { get; set; } = new();
        private CaseInventory CaseInventory { get; set; } = new();

        public CaseInventoryApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());

            GameItemFirst = new()
            {
                GameItemName = "GINCISTName",
                GameItemCost = 50000M,
                GameItemImage = "GIICISTImage",
                GameItemRarity = "Rare"
            };

            GameItemSecond = new()
            {
                GameItemName = "GINCISTName2",
                GameItemCost = 10M,
                GameItemImage = "GIICISTImage2",
                GameItemRarity = "Xyina"
            };

            GameCase = new()
            {
                GroupCasesName = "GCNCISTGroupName",
                GameCaseName = "GCNCISTName",
                GameCaseCost = 200M,
                GameCaseImage = "GCICISTImage",
                RevenuePrecentage = 10M
            };
        }
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULCIST{ipUser}User",
                UserEmail = $"ULCIST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULCIST{ipAdmin}Admin",
                UserEmail = $"ULCIST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULCIST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULCIST{ipAdmin}Admin");
        }

        [Fact]
        public async Task CaseInventorySimpleTests()
        {
            await InitializeOneTimeAccounts("0.2.0", "0.2.1");

            await CreateDependenciesInventory();
            
            Guid caseId = await SearchIdCase("Все или ничего");
            Guid firstItemId = await SearchIdItem("Драгон лор");
            Guid secondItemId = await SearchIdItem("Пистолет");
            
            CaseInventory caseInventory = new()
            {
                GameCaseId = caseId,
                GameItemId = firstItemId,
                LossChance = 1,
                NumberItemsCase = 1
            };
            
            //Create
            await CreateCaseInventory(caseInventory);

            caseInventory.GameItemId = secondItemId;
            caseInventory.LossChance = 99;
            
            await CreateCaseInventory(caseInventory);
            
            //Get and Get All
            CaseInventory caseInventoryFirst = await SearchCaseInventory(caseId, firstItemId);
            CaseInventory caseInventorySecond = await SearchCaseInventory(caseId, secondItemId);

            //Update
            caseInventoryFirst.NumberItemsCase = 2;
            
            await UpdateCaseInventory(caseInventoryFirst);
            
            //Delete
            await DeleteCaseInventory(caseInventoryFirst.Id);
            await DeleteCaseInventory(caseInventorySecond.Id);
            
            await DeleteDependenciesInventory(new() { caseInventoryFirst, caseInventorySecond });

            await DeleteOneTimeAccounts("0.2.0", "0.2.1");
        }

        private async Task<bool> CreateCaseInventory(CaseInventory caseInventory)
        {
            HttpStatusCode statusCode = await _clientApi.ResponsePost<CaseInventory>("/CaseInventory", caseInventory);

            return statusCode == HttpStatusCode.OK;
        }

        private async Task<CaseInventory> SearchCaseInventory(Guid caseId, Guid itemId)
        {
            var caseInventories = await _clientApi.ResponseGet<List<CaseInventory>>($"/CaseInventory/GetAll?caseId={caseId}");

            CaseInventory? caseInventory = caseInventories.FirstOrDefault(x => x.GameItemId == itemId);

            return caseInventory ?? throw new Exception("No such case inventory");
        }

        private async Task<bool> UpdateCaseInventory(CaseInventory caseInventory)
        {
            HttpStatusCode statusCode = await _clientApi.ResponsePut<CaseInventory>("/CaseInventory", caseInventory);

            return statusCode == HttpStatusCode.OK;
        }

        private async Task<bool> DeleteCaseInventory(Guid inventoryId)
        {
            HttpStatusCode statusCode = await _clientApi.ResponseDelete($"/CaseInventory?id={inventoryId}");
            
            return statusCode == HttpStatusCode.OK;
        }

        private async Task<bool> CreateDependenciesInventory()
        {
            HttpStatusCode createFirstItem = await _clientApi.ResponsePostStatusCode(
                "/GameItem", GameItemFirst, AdminTokens.AccessToken!);
            HttpStatusCode createSecondItem = await _clientApi.ResponsePostStatusCode(
                "/GameItem", GameItemSecond, AdminTokens.AccessToken!);
            HttpStatusCode createGameCase = await _clientApi.ResponsePostStatusCode(
                "/GameCase", GameCase, AdminTokens.AccessToken!);

            return (
                (createFirstItem == HttpStatusCode.OK) && 
                (createSecondItem == HttpStatusCode.OK) && 
                (createGameCase == HttpStatusCode.OK));
        }

        private async Task<Guid> SearchIdItem(string name)
        {
            GameItem? item = await _clientApi.ResponseGet<GameItem?>(
                $"/GameItem/GetByName?" +
                $"name={name}");

            if (item == null) throw new Exception("No such item"); 

            return item.Id;
        }

        private async Task<Guid> SearchIdCase(string name)
        {
            GameCase? gameCase = await _clientApi.ResponseGet<GameCase?>(
                $"/GameCase/GetByName?" +
                $"name={name}");

            if (gameCase == null) throw new Exception("No such game case");

            return gameCase.Id;
        }

        private async Task<bool> DeleteDependenciesInventory(List<CaseInventory> caseInventories)
        {
            HttpStatusCode statusCodeDeleteDependents = HttpStatusCode.BadRequest;

            foreach (CaseInventory inventory in caseInventories)
            {
                statusCodeDeleteDependents = await _clientApi.ResponseDelete($"/GameItem?id={inventory.GameItemId}");
            }

            statusCodeDeleteDependents = await _clientApi.ResponseDelete($"/GameCase?id={caseInventories[0].GameCaseId}");

            return statusCodeDeleteDependents == HttpStatusCode.OK;
        }
    }
}
