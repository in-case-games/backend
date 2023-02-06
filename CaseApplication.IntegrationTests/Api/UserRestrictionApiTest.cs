﻿using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Runtime.ConstrainedExecution;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRestrictionApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private TokenModel AdminToken { get; set; } = new();
        private User Admin { get; set; } = new();
        private AuthenticationTestHelper _authHelper;
        public UserRestrictionApiTest(WebApplicationFactory<Program> application, AuthenticationTestHelper authHelper)
        {
            _response = new ResponseHelper(application.CreateClient());
            _authHelper = authHelper;
        }
        private async Task InitializeOneTimeAccounts(string ipAdmin)
        {
            Admin = new()
            {
                UserLogin = $"ULURNIST{ipAdmin}Admin",
                UserEmail = $"UEURNIST{ipAdmin}Admin"
            };

            AdminToken = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }
        private async Task DeleteOneTimeAccounts(string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"UEURNIST{ipAdmin}Admin");
        }
        private async Task<UserRestriction> CreateUserRestriction()
        {
            User? currentUser = await _response
                .ResponseGet<User>($"/User/GetByLogin?login=UEURNIST0.8.1Admin",
                token: AdminToken.AccessToken!);
            UserRestriction userRestriction = new UserRestriction()
            {
                UserId = currentUser!.Id,
                RestrictionName = "testrest1",
                CreatedDate = DateTime.Now
            };

            return userRestriction;
        }
       
        [Fact]
        public async Task UserRestrictionCrudTest()
        {
            // Arrange
            await InitializeOneTimeAccounts("0.13.1");

            UserRestriction templateUserRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePostStatusCode("/UserRestriction",
                templateUserRestriction,
                token: AdminToken.AccessToken!);

            IEnumerable<UserRestriction>? userRestrictions = await _response
                .ResponseGet<List<UserRestriction>>
                ($"/UserRestriction/GetAll?userId={templateUserRestriction.UserId}",
                token: AdminToken.AccessToken!);
            UserRestriction userRestriction = userRestrictions!
                .FirstOrDefault(x => x.UserId == templateUserRestriction.UserId)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserRestriction?id={userRestriction.Id}",
                token: AdminToken.AccessToken!);
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/UserRestriction/GetAll?userId={userRestriction.UserId}",
                token: AdminToken.AccessToken!);

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/UserRestriction",
                userRestriction,
                token: AdminToken.AccessToken!);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/UserRestriction?id={userRestriction.Id}",
                token: AdminToken.AccessToken!);

            await _response.ResponseDelete($"/User?id={userRestriction.UserId}",
                token: AdminToken.AccessToken!);

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));

            await DeleteOneTimeAccounts("0.13.1");
        }
     
    }
}
