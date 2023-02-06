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
        private readonly AuthenticationTestHelper _authHelper;

        private TokenModel AdminTokens { get; set; } = new();
        private User Admin { get; set; } = new();

        private GameCase GameCase { get; set; } = new();
        private GameItem GameItemFirst { get; set; } = new();
        private GameItem GameItemSecond { get; set; } = new();

        public CaseInventoryApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
            _authHelper = new AuthenticationTestHelper(_clientApi);

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

        private async Task InitializeOneTimeAccount(string ipAdmin)
        {
            Admin = new()
            {
                UserLogin = $"ULCIST{ipAdmin}Admin",
                UserEmail = $"UECIST{ipAdmin}Admin"
            };

            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccount(string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULCIST{ipAdmin}Admin");
        }

        [Fact]
        public async Task CaseInventorySimpleTests()
        {
            await InitializeOneTimeAccount("0.2.0");

            await CreateDependenciesInventory();
            
            GameCase.Id = await SearchIdCase("GCNCISTName"); ;
            GameItemFirst.Id = await SearchIdItem("GINCISTName");
            GameItemSecond.Id = await SearchIdItem("GINCISTName2");

            CaseInventory caseInventory = new()
            {
                GameCaseId = GameCase.Id,
                GameItemId = GameItemFirst.Id,
                LossChance = 1,
                NumberItemsCase = 1
            };
            
            //Create
            await CreateCaseInventory(caseInventory);

            caseInventory.GameItemId = GameItemSecond.Id;
            caseInventory.LossChance = 99;
            
            await CreateCaseInventory(caseInventory);
            
            //Get and Get All
            CaseInventory caseInventoryFirst = await SearchCaseInventory(
                GameCase.Id, 
                GameItemFirst.Id);
            CaseInventory caseInventorySecond = await SearchCaseInventory(
                GameCase.Id, 
                GameItemSecond.Id);

            //Update
            caseInventoryFirst.LossChance = 2;
            await UpdateCaseInventory(caseInventoryFirst);
            
            //Delete
            await DeleteCaseInventory(caseInventoryFirst.Id);
            await DeleteCaseInventory(caseInventorySecond.Id);
            
            await DeleteDependenciesInventory(new() { caseInventoryFirst, caseInventorySecond });

            await DeleteOneTimeAccount("0.2.0");
        }

        private async Task<bool> CreateCaseInventory(CaseInventory caseInventory)
        {
            HttpStatusCode statusCode = await _clientApi.ResponsePostStatusCode(
                "/CaseInventory", caseInventory, AdminTokens.AccessToken!);

            return statusCode == HttpStatusCode.OK;
        }

        private async Task<CaseInventory> SearchCaseInventory(Guid caseId, Guid itemId)
        {
            CaseInventory? inventory = await _clientApi.ResponseGet<CaseInventory>(
                $"/CaseInventory/GetById?" +
                $"caseId={caseId}&" +
                $"itemId={itemId}");

            if (inventory == null) throw new Exception("No such case inventory");

            return inventory;
        }

        private async Task<bool> UpdateCaseInventory(CaseInventory caseInventory)
        {
            HttpStatusCode statusCode = await _clientApi.ResponsePut(
                "/CaseInventory", caseInventory, AdminTokens.AccessToken!);

            return statusCode == HttpStatusCode.OK;
        }

        private async Task<bool> DeleteCaseInventory(Guid id)
        {
            HttpStatusCode statusCode = await _clientApi.ResponseDelete(
                $"/CaseInventory?id={id}", AdminTokens.AccessToken!);
            
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
            HttpStatusCode codeDelete;

            foreach (CaseInventory inventory in caseInventories)
            {
                codeDelete = await _clientApi.ResponseDelete(
                    $"/GameItem?id={inventory.GameItemId}", AdminTokens.AccessToken!);
            }

            codeDelete = await _clientApi.ResponseDelete(
                $"/GameCase?id={caseInventories[0].GameCaseId}", AdminTokens.AccessToken!);

            return codeDelete == HttpStatusCode.OK;
        }
    }
}
