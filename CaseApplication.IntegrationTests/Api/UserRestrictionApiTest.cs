﻿using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRestrictionApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserRestrictionApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task<UserRestriction> CreateUserRestriction()
        {
            User user = InitializeUser();
            await _response.ResponsePost<User>("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User/GetByEmail?email={user.UserEmail}&hash=123");
            UserRestriction userRestriction = new UserRestriction()
            {
                UserId = currentUser.Id,
                RestrictionName = "testrest1",
                CreatedDate = DateTime.Now
            };

            return userRestriction;
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = "testlogin2",
                UserEmail = "testlogin2",
                UserImage = "testlogin2",
                PasswordHash = "testlogin2",
                PasswordSalt = "testlogin2",
            };

            return user;
        }
        [Fact]
        public async Task UserRestrictionCrudTest()
        {
            // Arrange
            UserRestriction templateUserRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost<UserRestriction>("/UserRestriction", templateUserRestriction);

            IEnumerable<UserRestriction> userRestrictions = await _response
                .ResponseGet<List<UserRestriction>>
                ($"/UserRestriction/GetAll?userId={templateUserRestriction.UserId}");
            UserRestriction userRestriction = userRestrictions
                .FirstOrDefault(x => x.UserId == templateUserRestriction.UserId)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/UserRestriction?id={userRestriction.Id}");
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/UserRestriction/GetAll?userId={userRestriction.UserId}");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut<UserRestriction>("/UserRestriction", userRestriction);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/UserRestriction?id={userRestriction.Id}");

            await _response.ResponseDelete($"/User?id={userRestriction.UserId}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));
        }
     
    }
}
