using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameCaseApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ClientApiRepository _clientApi;

        public GameCaseApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }


        [Fact]
        public async Task GameCaseCreateTest()
        {
            PostEntityModel<GameCase> postEntityModel = new()
            {
                PostUrl = "/GameCase",
                PostContent = new()
                {
                    GameCaseCost = 0,
                    GameCaseName = "Test",
                    GameCaseImage = "Image",
                    RevenuePrecentage = 0
                }
            };

            HttpStatusCode statusCode = await _clientApi.CreateResponsePost<GameCase>(postEntityModel);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameCaseGetTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            GameCase gameCase = await _clientApi.CreateResponseGet<GameCase>($"/GameCase?id={caseId}");

            Assert.Equal(gameCase.Id, caseId);
        }

        [Fact]
        public async Task GameCaseUpdateTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            PostEntityModel<GameCase> postEntityModel = new()
            {
                PostUrl = "/GameCase",
                PostContent = new()
                {
                    Id = caseId,
                    GameCaseCost = 2M,
                    GameCaseName = "Test123",
                    GameCaseImage = "Image321"
                }
            };

            HttpStatusCode statusCode = await _clientApi.CreateResponsePut<GameCase>(postEntityModel);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameCaseDeleteTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            HttpStatusCode statusCode = await _clientApi.CreateResponseDelete($"/GameCase?id={caseId}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        private async Task<Guid> SearchIdLastGameCase()
        {
            List<GameCase> gameCases = await _clientApi.CreateResponseGet<List<GameCase>>("/GameCase/GetAllCases");
            return gameCases.LastOrDefault()!.Id;
        }
    }
}
