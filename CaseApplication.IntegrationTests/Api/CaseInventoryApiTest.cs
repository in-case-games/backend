using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.IntegrationTests.Api
{
    public class CaseInventoryApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ClientApiRepository _clientApi;

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

                PostEntityModel<CaseInventory> entityModel = new()
                {
                    PostUrl = "/CaseInventory",
                    PostContent = new CaseInventory()
                    {
                        GameCaseId = caseId,
                        GameItemId = firstItemId,
                        LossChance = 0,
                        NumberItemsCase = 1
                    }
                };

                createFirstInventory = await _clientApi.CreateResponsePost<CaseInventory>(entityModel);

                entityModel.PostContent.GameItemId = secondItemId;
                entityModel.PostContent.LossChance = 0;

                createSecondInventory = await _clientApi.CreateResponsePost<CaseInventory>(entityModel);
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
                List<CaseInventory> inventories = await _clientApi.CreateResponseGet<List<CaseInventory>>($"/CaseInventory/GetAllCaseInventories?caseId={caseId}");
                idFirstInventory = inventories[0].Id;
                idSecondInventory = inventories[1].Id;

                getInventoryTest1 = await _clientApi.CreateResponseGet<CaseInventory>($"/CaseInventory?id={idFirstInventory}");
                getInventoryTest2 = await _clientApi.CreateResponseGet<CaseInventory>($"/CaseInventory?id={idSecondInventory}");
            }

            //Update Test
            bool IsGetInventory = ((getInventoryTest1.Id == idFirstInventory) && 
                (getInventoryTest2.Id == idSecondInventory));

            if (IsGetInventory)
            {
                PostEntityModel<CaseInventory> entityModel = new()
                {
                    PostUrl = "/CaseInventory",
                    PostContent = new CaseInventory()
                    {
                        Id = idFirstInventory,
                        GameCaseId = caseId,
                        GameItemId = getInventoryTest1.GameItemId,
                        LossChance = 1,
                        NumberItemsCase = 2
                    }
                }; 

                statusUpdate = await _clientApi.CreateResponsePut<CaseInventory>(entityModel);
            }

            //Delete Test
            bool IsUpdateInventory = statusUpdate == HttpStatusCode.OK;

            if(IsUpdateInventory)
            {
                statusDelete = await _clientApi.CreateResponseDelete($"/CaseInventory?id={idFirstInventory}");
                statusDelete = await _clientApi.CreateResponseDelete($"/CaseInventory?id={idSecondInventory}");
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
            List<GameItem> items = await _clientApi.CreateResponseGet<List<GameItem>>("/GameItem/GetAllItems");

            return items.FirstOrDefault(x => x.GameItemName == name)!.Id;
        }

        private async Task<Guid> SearchIdCase(string name)
        {
            List<GameCase> items = await _clientApi.CreateResponseGet<List<GameCase>>("/GameCase/GetAllCases");

            return items.FirstOrDefault(x => x.GameCaseName == name)!.Id;
        }

        private async Task<bool> DeleteDependentsInventory(List<CaseInventory> caseInventories)
        {
            HttpStatusCode statusCodeDeleteDependents = HttpStatusCode.BadRequest;

            foreach (CaseInventory inventory in caseInventories)
            {
                statusCodeDeleteDependents = await _clientApi.CreateResponseDelete($"/GameItem?id={inventory.GameItemId}");
            }

            statusCodeDeleteDependents = await _clientApi.CreateResponseDelete($"/GameCase?id={caseInventories[0].GameCaseId}");

            return statusCodeDeleteDependents == HttpStatusCode.OK;
        }

        private async Task<bool> CreateDependentsInventory()
        {
            PostEntityModel<GameItem> entityModelItem = new()
            {
                PostUrl = "/GameItem",
                PostContent = new GameItem()
                {
                    GameItemName = "Драгон лор",
                    GameItemCost = 50,
                    GameItemImage = "aaa",
                    GameItemRarity = "Редкий"
                }
            };

            PostEntityModel<GameItem> entityModelItem2 = new()
            {
                PostUrl = "/GameItem",
                PostContent = new GameItem()
                {
                    GameItemName = "Пистолет",
                    GameItemCost = 1,
                    GameItemImage = "abc",
                    GameItemRarity = "Ширп"
                }
            };

            PostEntityModel<GameCase> entityModelCase = new()
            {
                PostUrl = "/GameCase",
                PostContent = new GameCase()
                {
                    GameCaseName = "Все или ничего",
                    GameCaseCost = 20,
                    GameCaseImage = "asd",
                    RevenuePrecentage = 0
                }
            };

            HttpStatusCode createFirstItem = await _clientApi.CreateResponsePost(entityModelItem);
            HttpStatusCode createSecondItem = await _clientApi.CreateResponsePost(entityModelItem2);
            HttpStatusCode createGameCase = await _clientApi.CreateResponsePost(entityModelCase);

            return (
                (createFirstItem == HttpStatusCode.OK) && 
                (createSecondItem == HttpStatusCode.OK) && 
                (createGameCase == HttpStatusCode.OK));
        }
    }
}
