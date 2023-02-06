using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserHistoryOpeningCasesApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private TokenModel AdminToken { get; set; } = new();
        private User Admin { get; set; } = new();
        private User User { get; set; } = new();
        private AuthenticationTestHelper _authHelper;

        public UserHistoryOpeningCasesApiTest(WebApplicationFactory<Program> applicationFactory,
            AuthenticationTestHelper helper)
        {
            _clientApi = new(applicationFactory.CreateClient());
            _authHelper = helper;
        }
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULUIAT{ipUser}User",
                UserEmail = $"UEUIAT{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULUIAT{ipAdmin}Admin",
                UserEmail = $"UEUIAT{ipAdmin}Admin"
            };

            AdminToken = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }
        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"UEUIAT{ipAdmin}Admin");
        }

        [Fact]
        public async Task UserHistoryOpeningCasesSimpleTest()
        {
            await InitializeOneTimeAccounts("0.11.0", "0.11.1");
            bool IsCreatedDependencies = await CreateDependenciesUserHistory();
            bool IsDeletedDependencies = false;

            Guid userId = await SearchIdUser(User!.UserEmail!);

            if (IsCreatedDependencies)
            {
                Guid caseId = await SearchIdCase("UHOCCaseName");
                Guid itemId = await SearchIdItem("UHOCItemName");
                Guid item2Id = await SearchIdItem("UHOCItemName2");

                UserHistoryOpeningCases userHistory = new()
                {
                    GameCaseId = caseId,
                    GameItemId = itemId,
                    UserId = userId,
                    CaseOpenAt = DateTime.UtcNow
                };

                //Create
                await CreateUserHistory(userHistory);
                userHistory.GameItemId = item2Id;
                await CreateUserHistory(userHistory);

                //Get and Get All
                List<UserHistoryOpeningCases> userHistories = await SearchUserHistory(userId);
                UserHistoryOpeningCases fisrtUserHistory = userHistories[0];
                UserHistoryOpeningCases secondUserHistory = userHistories[1];

                //Delete
                await DeleteUserHistory(fisrtUserHistory.Id);
                await DeleteUserHistory(secondUserHistory.Id);
                IsDeletedDependencies = await DeleteDependenciesUserHistory(userHistories);
            }

            Assert.True(IsDeletedDependencies);

            await DeleteOneTimeAccounts("0.11.0", "0.11.1");
        }
        private async Task<List<UserHistoryOpeningCases>> SearchUserHistory(Guid userId)
        {
            List<UserHistoryOpeningCases>? caseInventories = await _clientApi
                .ResponseGet<List<UserHistoryOpeningCases>>
                ($"/UserHistoryOpeningCases/GetAllById?userId={userId}",
                token: AdminToken.AccessToken!);

            return caseInventories ?? throw new Exception("No such case inventory");
        }
        private async Task<bool> CreateUserHistory(UserHistoryOpeningCases userHistory)
        {
            HttpStatusCode statusCode = await _clientApi
            .ResponsePostStatusCode<UserHistoryOpeningCases>("/UserHistoryOpeningCases",
            userHistory,
            token: AdminToken.AccessToken!);

            return statusCode == HttpStatusCode.OK;
        }
        private async Task<bool> DeleteUserHistory(Guid id)
        {
            HttpStatusCode statusCode = await _clientApi.ResponseDelete($"/UserHistoryOpeningCases?id={id}",
                token: AdminToken.AccessToken!);

            return statusCode == HttpStatusCode.OK;
        }

        private async Task<bool> CreateDependenciesUserHistory()
        {
            GameItem gameItemFirst = new()
            {
                GameItemName = "UHOCItemName",
                GameItemCost = 40000,
                GameItemImage = "UHOCItemImage",
                GameItemRarity = "Редкий"
            };
            GameItem gameItemSecond = new()
            {
                GameItemName = "UHOCItemName2",
                GameItemCost = 34000,
                GameItemImage = "UHOCItemImage2",
                GameItemRarity = "Редкий"
            };
            GameCase gameCase = new()
            {
                GroupCasesName = "Кейсы для бедных",
                GameCaseName = "UHOCCaseName",
                GameCaseCost = 20,
                GameCaseImage = "UHOCCaseImage",
                RevenuePrecentage = 0
            };

            User user = new()
            {
                UserLogin = "UHOCUserName",
                UserEmail = "UHOCUserEmail@mail.ru",
                UserImage = "UHOCUserImage",
            };

            HttpStatusCode createUser = await _clientApi
                .ResponsePostStatusCode(
                "/User?password=1234",
                user,
                token: AdminToken.AccessToken!);
            HttpStatusCode createFirstItem = await _clientApi
                .ResponsePostStatusCode("/GameItem",
                gameItemFirst,
                token: AdminToken.AccessToken!);
            HttpStatusCode createSecondItem = await _clientApi
                .ResponsePostStatusCode("/GameItem",
                gameItemSecond,
                token: AdminToken.AccessToken!);
            HttpStatusCode createGameCase = await _clientApi
                .ResponsePostStatusCode("/GameCase",
                gameCase,
                token: AdminToken.AccessToken!);

            return (
                (createFirstItem == HttpStatusCode.OK) &&
                (createSecondItem == HttpStatusCode.OK) &&
                (createUser == HttpStatusCode.OK) &&
                (createGameCase == HttpStatusCode.OK));
        }

        private async Task<Guid> SearchIdItem(string name)
        {
            List<GameItem>? items = await _clientApi.ResponseGet<List<GameItem>>("/GameItem/GetAll",
                token: AdminToken.AccessToken!);

            return items!.FirstOrDefault(x => x.GameItemName == name)!.Id;
        }

        private async Task<Guid> SearchIdCase(string name)
        {
            List<GameCase>? items = await _clientApi.ResponseGet<List<GameCase>>("/GameCase/GetAll",
                token: AdminToken.AccessToken!);

            return items!.FirstOrDefault(x => x.GameCaseName == name)!.Id;
        }

        private async Task<Guid> SearchIdUser(string login)
        {
            User? user = await _clientApi.ResponseGet<User>($"/User/GetByLogin?email={login}&hash=123",
                token: AdminToken.AccessToken!);

            return user!.Id;
        }
        private async Task<bool> DeleteDependenciesUserHistory(List<UserHistoryOpeningCases> userHistories)
        {
            HttpStatusCode statusCodeDeleteDependents = HttpStatusCode.BadRequest;

            foreach (UserHistoryOpeningCases history in userHistories)
            {
                statusCodeDeleteDependents = await _clientApi
                    .ResponseDelete($"/GameItem?id={history.GameItemId}",
                    token: AdminToken.AccessToken!);
            }

            statusCodeDeleteDependents = await _clientApi
                .ResponseDelete($"/GameCase?id={userHistories[0].GameCaseId}",
                token: AdminToken.AccessToken!);
            statusCodeDeleteDependents = await _clientApi
                .ResponseDelete($"/User?id={userHistories[0].UserId}",
                token: AdminToken.AccessToken!);

            return statusCodeDeleteDependents == HttpStatusCode.OK;
        }
    }
}
