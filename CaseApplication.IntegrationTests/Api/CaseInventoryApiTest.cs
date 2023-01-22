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
        public async Task CaseInventoryTests()
        {
            //Create Test
            bool IsCreateDependent = await CreateDependentsInventory();

            HttpStatusCode createFirstInventory = HttpStatusCode.BadRequest;
            HttpStatusCode createSecondInventory = HttpStatusCode.BadRequest;
            HttpStatusCode statusUpdate = HttpStatusCode.BadRequest;
            HttpStatusCode statusDelete = HttpStatusCode.BadRequest;

            Guid caseId = new Guid();

            if (IsCreateDependent)
            {
                Guid firstItemId = await SearchIdItem("Драгон лор");
                Guid secondItemId = await SearchIdItem("Пистолет");
                caseId = await SearchIdCase("Все или ничего");

                CaseInventory caseInventory = new()
                {
                    GameCaseId = caseId,
                    GameItemId = firstItemId,
                    LossChance = 0.01M,
                    NumberItemsCase = 1
                };

                createFirstInventory = await _clientApi.ResponsePost<CaseInventory>("/CaseInventory", caseInventory);

                caseInventory.GameItemId = secondItemId;
                caseInventory.LossChance = 0.99M;

                createSecondInventory = await _clientApi.ResponsePost<CaseInventory>("/CaseInventory", caseInventory);
            }

            //Get Test and Get All

            bool IsCreatedInventory = (createFirstInventory == HttpStatusCode.OK) &&
                (createSecondInventory == HttpStatusCode.OK);

            Guid idFirstInventory = new Guid();
            Guid idSecondInventory = new Guid();
            CaseInventory getInventoryTest1 = new();
            CaseInventory getInventoryTest2 = new();

            if (IsCreatedInventory)
            {
                List<CaseInventory> inventories = await _clientApi.ResponseGet<List<CaseInventory>>($"/CaseInventory/GetAllCaseInventories?caseId={caseId}");
                idFirstInventory = inventories[0].Id;
                idSecondInventory = inventories[1].Id;

                getInventoryTest1 = await _clientApi.ResponseGet<CaseInventory>($"/CaseInventory?id={idFirstInventory}");
                getInventoryTest2 = await _clientApi.ResponseGet<CaseInventory>($"/CaseInventory?id={idSecondInventory}");
            }

            //Update Test
            bool IsGetInventory = ((getInventoryTest1.Id == idFirstInventory) && 
                (getInventoryTest2.Id == idSecondInventory));

            if (IsGetInventory)
            {
                CaseInventory caseInventory = new()
                {
                    Id = idFirstInventory,
                    GameCaseId = caseId,
                    GameItemId = getInventoryTest1.GameItemId,
                    LossChance = 0.01M,
                    NumberItemsCase = 2
                };

                statusUpdate = await _clientApi.ResponsePut<CaseInventory>("/CaseInventory", caseInventory);
            }

            //Delete Test
            bool IsUpdateInventory = statusUpdate == HttpStatusCode.OK;

            if(IsUpdateInventory)
            {
                statusDelete = await _clientApi.ResponseDelete($"/CaseInventory?id={idFirstInventory}");
                statusDelete = await _clientApi.ResponseDelete($"/CaseInventory?id={idSecondInventory}");
            }

            bool IsDeleteInventory = statusDelete == HttpStatusCode.OK;
            bool IsDeleteDependens = false;

            if(IsDeleteInventory)
            {
                IsDeleteDependens = await DeleteDependentsInventory(new() { getInventoryTest1, getInventoryTest2 });
            }

            Assert.True(IsDeleteDependens);
        }

        private async Task<Guid> SearchIdItem(string name)
        {
            List<GameItem> items = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAllItems");

            return items.FirstOrDefault(x => x.GameItemName == name)!.Id;
        }

        private async Task<Guid> SearchIdCase(string name)
        {
            List<GameCase> items = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAllCases");

            return items.FirstOrDefault(x => x.GameCaseName == name)!.Id;
        }

        private async Task<bool> DeleteDependentsInventory(List<CaseInventory> caseInventories)
        {
            HttpStatusCode statusCodeDeleteDependents = HttpStatusCode.BadRequest;

            foreach (CaseInventory inventory in caseInventories)
            {
                statusCodeDeleteDependents = await _clientApi.ResponseDelete($"/GameItem?id={inventory.GameItemId}");
            }

            statusCodeDeleteDependents = await _clientApi.ResponseDelete($"/GameCase?id={caseInventories[0].GameCaseId}");

            return statusCodeDeleteDependents == HttpStatusCode.OK;
        }

        private async Task<bool> CreateDependentsInventory()
        {
            GameItem gameItem = new()
            {
                GameItemName = "Драгон лор",
                GameItemCost = 500000,
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
    }
}
