using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class CaseInventoryApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;

        public CaseInventoryApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task CaseInventorySimpleTests()
        {
            bool IsCreatedDependencies = await CreateDependenciesInventory();
            bool IsDeletedDependencies = false;

            if (IsCreatedDependencies)
            {
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

                IsDeletedDependencies = await DeleteDependenciesInventory(new() { caseInventoryFirst, caseInventorySecond });
            }

            Assert.True(IsDeletedDependencies);
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
            GameItem gameItem = new()
            {
                GameItemName = "Драгон лор",
                GameItemCost = 50000,
                GameItemImage = "aaa",
                GameItemRarity = "Редкий"
            };

            GameItem gameItem2 = new()
            {
                GameItemName = "Пистолет",
                GameItemCost = 10,
                GameItemImage = "abc",
                GameItemRarity = "Ширп"
            };

            GameCase gameCase = new()
            {
                GroupCasesName = "Бедные кейсы",
                GameCaseName = "Все или ничего",
                GameCaseCost = 20,
                GameCaseImage = "asd",
                RevenuePrecentage = 0
            };

            HttpStatusCode createFirstItem = await _clientApi.ResponsePost("/GameItem", gameItem);
            HttpStatusCode createSecondItem = await _clientApi.ResponsePost("/GameItem", gameItem2);
            HttpStatusCode createGameCase = await _clientApi.ResponsePost("/GameCase", gameCase);

            return (
                (createFirstItem == HttpStatusCode.OK) && 
                (createSecondItem == HttpStatusCode.OK) && 
                (createGameCase == HttpStatusCode.OK));
        }

        private async Task<Guid> SearchIdItem(string name)
        {
            List<GameItem> items = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAll");

            return items.FirstOrDefault(x => x.GameItemName == name)!.Id;
        }

        private async Task<Guid> SearchIdCase(string name)
        {
            List<GameCase> items = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAll");

            return items.FirstOrDefault(x => x.GameCaseName == name)!.Id;
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
