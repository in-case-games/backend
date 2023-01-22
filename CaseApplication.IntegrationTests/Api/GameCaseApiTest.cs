using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameCaseApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private Random random = new();

        public GameCaseApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }


        [Fact]
        public async Task GameCaseCreateTest()
        {
            GameCase gameCase = new()
            {
                GameCaseCost = 0,
                GameCaseName = $"Test {random.Next(1, 1000000)}",
                GameCaseImage = "Image",
                RevenuePrecentage = 0
            };

            HttpStatusCode statusCode = await _clientApi.ResponsePost<GameCase>("/GameCase", gameCase);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameCaseGetTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            GameCase gameCase = await _clientApi.ResponseGet<GameCase>($"/GameCase?id={caseId}");

            Assert.Equal(gameCase.Id, caseId);
        }

        [Fact]
        public async Task GameCaseGetAllTest()
        {
            List<GameCase> gameCases = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAllCases");

            Assert.True(gameCases.Count > 0);
        }

        [Fact]
        public async Task GameCaseUpdateTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            GameCase gameCase = new()
            {
                Id = caseId,
                GameCaseCost = 200,
                GameCaseName = $"Test {random.Next(1, 1000000)}",
                GameCaseImage = "Image321"
            };

            HttpStatusCode statusCode = await _clientApi.ResponsePut<GameCase>("/GameCase", gameCase);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GameCaseDeleteTest()
        {
            Guid caseId = await SearchIdLastGameCase();

            HttpStatusCode statusCode = await _clientApi.ResponseDelete($"/GameCase?id={caseId}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        private async Task<Guid> SearchIdLastGameCase()
        {
            List<GameCase> gameCases = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAllCases");
            
            return gameCases.LastOrDefault()!.Id;
        }
    }
}
