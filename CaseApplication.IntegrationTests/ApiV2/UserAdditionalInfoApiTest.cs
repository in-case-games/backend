using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserAdditionalInfoApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserAdditionalInfoApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }
        [Fact]
        public async Task GET_GetAuthorizeUserInfo_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/UserAdditionalInfo/", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
        [Fact]
        public async Task PUT_UpdateUserByAdmin_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);
            UserAdditionalInfo? info = await _response
                .ResponseGet<UserAdditionalInfo>("/UserAdditionalInfo/", AccessToken);
            
            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut("/UserAdditionalInfo/admin", info!, AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
        private async Task<UserAdditionalInfo> CreateUser(Guid guid)
        {
            UserRole? userRole = await Context.UserRole.FirstOrDefaultAsync(x => x.RoleName == "user");
            UserAdditionalInfo userInfo = new()
            {
                IsConfirmedAccount = true,
                UserBalance = 99999999M,
                UserRole = userRole!,
                UserRoleId = userRole!.Id,
                UserId = guid
            };
            User user = new()
            {
                Id = guid,
                UserLogin = "UserAdditional_Info/Test",
                UserEmail = "testsstst////y@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserAdditional1",
                UserAdditionalInfo = userInfo,
            };

            await Context.User.AddAsync(user);
            await Context.UserAdditionalInfo.AddAsync(userInfo);
            await Context.SaveChangesAsync();

            return userInfo;
        }
    }
}
