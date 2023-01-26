using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserHistoryOpeningCasesApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;

        public UserHistoryOpeningCasesApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task UserHistoryOpeningCasesSimpleTest()
        {
            bool IsCreatedDependencies = await CreateDependenciesUserHistory();
            bool IsDeletedDependencies = false;

            Guid userId = await SearchIdUser("UHOCUserEmail@mail.ru");

            if (IsCreatedDependencies)
            {
                Guid caseId = await SearchIdCase("UHOCCaseName");
                Guid itemId = await SearchIdItem("UHOCItemName");
                Guid item2Id = await SearchIdItem("UHOCItemName2");

                UserHistoryOpeningCases userHistory = new()
                {
                    GameCaseId = caseId,
                    GameItemId = itemId,
                    UserId = userId
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
        }
        private async Task<List<UserHistoryOpeningCases>> SearchUserHistory(Guid userId)
        {
            List<UserHistoryOpeningCases> caseInventories = await _clientApi
                .ResponseGet<List<UserHistoryOpeningCases>>
                ($"/UserHistoryOpeningCases/GetAll?userId={userId}");

            return caseInventories ?? throw new Exception("No such case inventory");
        }
        private async Task<bool> CreateUserHistory(UserHistoryOpeningCases userHistory)
        {
            HttpStatusCode statusCode = await _clientApi
                .ResponsePost<UserHistoryOpeningCases>("/UserHistoryOpeningCases", userHistory);

            return statusCode == HttpStatusCode.OK;
        }
        private async Task<bool> DeleteUserHistory(Guid id)
        {
            HttpStatusCode statusCode = await _clientApi.ResponseDelete($"/UserHistoryOpeningCases?id={id}");

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

            HttpStatusCode createUser = await _clientApi.ResponsePost("/User", user);
            HttpStatusCode createFirstItem = await _clientApi.ResponsePost("/GameItem", gameItemFirst);
            HttpStatusCode createSecondItem = await _clientApi.ResponsePost("/GameItem", gameItemSecond);
            HttpStatusCode createGameCase = await _clientApi.ResponsePost("/GameCase", gameCase);

            return (
                (createFirstItem == HttpStatusCode.OK) &&
                (createSecondItem == HttpStatusCode.OK) &&
                (createUser == HttpStatusCode.OK) &&
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

        private async Task<Guid> SearchIdUser(string email)
        {
            User user = await _clientApi.ResponseGet<User>($"/User?email={email}&hash=123");

            return user.Id;
        }
        private async Task<bool> DeleteDependenciesUserHistory(List<UserHistoryOpeningCases> userHistories)
        {
            HttpStatusCode statusCodeDeleteDependents = HttpStatusCode.BadRequest;

            foreach (UserHistoryOpeningCases history in userHistories)
            {
                statusCodeDeleteDependents = await _clientApi
                    .ResponseDelete($"/GameItem?id={history.GameItemId}");
            }

            statusCodeDeleteDependents = await _clientApi
                .ResponseDelete($"/GameCase?id={userHistories[0].GameCaseId}");
            statusCodeDeleteDependents = await _clientApi
                .ResponseDelete($"/User?id={userHistories[0].UserId}");

            return statusCodeDeleteDependents == HttpStatusCode.OK;
        }
    }
}
