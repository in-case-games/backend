using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameCaseApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();

        private GameCase GameCase { get; set; } = new();

        private Random random = new();

        public GameCaseApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());

            GameCase = new()
            {
                GroupCasesName = "GCNGCSTGroupName",
                GameCaseCost = 200M,
                GameCaseName = $"GCNGCSTCaseName",
                GameCaseImage = "GCIGCSTImgae",
                RevenuePrecentage = 10M
            };
        }

        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULGCST{ipUser}User",
                UserEmail = $"UEGCST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULGCST{ipAdmin}Admin",
                UserEmail = $"UEGCST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULGCST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULGCST{ipAdmin}Admin");
        }

        [Fact]
        public async Task GameCaseSimpleTests()
        {
            await InitializeOneTimeAccounts("0.1.0", "0.1.1");

            //Post
            HttpStatusCode statusCodeCreate = await _clientApi.ResponsePostStatusCode(
                "/GameCase", GameCase, AdminTokens.AccessToken!);

            //Get && Get All
            Guid caseId = await SearchIdCaseByName(GameCase.GameCaseName!);

            HttpStatusCode getByGroupNameStatusCode = await _clientApi.ResponseGetStatusCode(
                $"/GameCase/GetAllByGroupName?" +
                $"name={GameCase.GroupCasesName}");

            //Put
            GameCase.Id = caseId;
            GameCase.GameCaseCost = 201M;

            HttpStatusCode statusCodePut = await _clientApi.ResponsePut(
                "/GameCase", GameCase, AdminTokens.AccessToken!);

            //Delete
            HttpStatusCode statusCodeDelete = await _clientApi.ResponseDelete(
                $"/GameCase?id={caseId}", AdminTokens.AccessToken!);

            bool IsPassedTests = (statusCodeCreate == HttpStatusCode.OK) &&
                (statusCodePut == HttpStatusCode.OK) &&
                (statusCodeDelete == HttpStatusCode.OK) &&
                (getByGroupNameStatusCode == HttpStatusCode.OK);

            await DeleteOneTimeAccounts("0.1.0", "0.1.1");

            Assert.True(IsPassedTests);
        }

        private async Task<Guid> SearchIdCaseByName(string name)
        {
            GameCase? gameCase = await _clientApi.ResponseGet<GameCase?>(
                "/GameCase/GetByName?" +
                $"name={name}");

            if (gameCase == null) throw new Exception("No such case :)");

            return gameCase.Id;
        }
    }
}
