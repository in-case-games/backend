using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Reflection.Metadata.Ecma335;

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
        public async Task GameCaseTests()
        {
            //Post
            GameCase gameCase = new()
            {
                GameCaseCost = 200M,
                GameCaseName = $"Test {random.Next(1, 1000000)}",
                GameCaseImage = "image541",
                RevenuePrecentage = 10M
            };

            HttpStatusCode statusCodeCreate = await _clientApi.ResponsePost<GameCase>("/GameCase", gameCase);
            
            //Get && Get All
            Guid caseId = await SearchIdCaseByName(gameCase.GameCaseName);

            //Put
            gameCase.Id = caseId;
            gameCase.GameCaseImage = "igame4123";

            HttpStatusCode statusCodePut = await _clientApi.ResponsePut<GameCase>("/GameCase", gameCase);

            //Delete
            HttpStatusCode statusCodeDelete = await _clientApi.ResponseDelete($"/GameCase?id={caseId}");

            bool IsPassedTests = (statusCodeCreate == HttpStatusCode.OK) &&
                (statusCodePut == HttpStatusCode.OK) &&
                (statusCodeDelete == HttpStatusCode.OK);

            Assert.True(IsPassedTests);
        }

        private async Task<Guid> SearchIdCaseByName(string name)
        {
            List<GameCase> gameCases = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAllCases");
            
            GameCase gameCase = gameCases.FirstOrDefault(x => x.GameCaseName == name) ?? new();

            return gameCase.Id;
        }
    }
}
