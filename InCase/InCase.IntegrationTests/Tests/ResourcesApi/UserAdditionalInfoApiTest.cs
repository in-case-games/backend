using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class UserAdditionalInfoApiTest : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();
        private readonly Guid UserGuid = Guid.NewGuid();
        public UserAdditionalInfoApiTest(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Theory]
        [InlineData("/api/user-additional-info/guest-mode")]
        [InlineData("/api/user-additional-info")]
        public async Task GET_TurnOnOrTurnOffGuestModeButUnauthorized_Unauthorized(string uri)
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }
        [Fact]
        public async Task GET_GetAdditionalInfo_OK()
        {
            // Arrange
            await InitializeUserDependency(UserGuid);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-additional-info", AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_GetNotExistedAdditionalInfo_NotFound()
        {
            // Arrange
            string fakeAccessToken = await CreateFakeToken();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-additional-info", fakeAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_GetUserRoles_OK()
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-additional-info/roles");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_TurnOnOrTurnOffGuestMode_True()
        {
            // Arrange
            await InitializeUserDependency(UserGuid, "bot");

            // Act
            string uri = "/api/user-additional-info/guest-mode";

            HttpStatusCode getTurnOnGuestMode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            HttpStatusCode getTurnOffGuestMode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.True(getTurnOffGuestMode == getTurnOnGuestMode);
        }
        [Fact]
        public async Task GET_TurnOnOrTurnOffGuestModeButAccessDenied_Forbidden()
        {
            // Arrange
            string fakeAccessToken = await CreateFakeToken("user");

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-additional-info/guest-mode", fakeAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, getStatusCode);
        }
        [Theory]
        [InlineData(Roles.User, HttpStatusCode.Forbidden)]
        [InlineData(Roles.Admin, HttpStatusCode.Forbidden)]
        [InlineData(Roles.Owner, HttpStatusCode.OK)]
        [InlineData(Roles.Bot, HttpStatusCode.OK)]
        public async Task PUT_UpdateAdditionalInfo(string roleName, HttpStatusCode result)
        {
            // Arrange
            await InitializeUserDependency(UserGuid, roleName);

            UserRole? userRole = await Context.UserRoles
                .FirstOrDefaultAsync(x => x.Name == roleName);
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == UserGuid);

            UserAdditionalInfo userInfo = new()
            {
                Id = user!.AdditionalInfo!.Id,
                IsConfirmed = true,
                Balance = 9999,
                Role = userRole!,
                RoleId = userRole!.Id,
                User = user,
                UserId = UserGuid,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-additional-info", userInfo.Convert(false), AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.Equal(result, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateAdditionalInfoButUnauthorized_Unauthorized()
        {
            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == "bot");

            UserAdditionalInfo userInfo = new()
            {
                IsConfirmed = true,
                Balance = 9999,
                Role = userRole!,
                RoleId = userRole!.Id,
                UserId = UserGuid,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-additional-info", userInfo);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateAdditionalInfoButRoleIsNotExist_NotFound()
        {
            // Arrange
            await InitializeUserDependency(UserGuid, "bot");

            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == UserGuid);

            UserAdditionalInfo userInfo = new()
            {
                Id = user!.AdditionalInfo!.Id,
                IsConfirmed = true,
                Balance = 9999,
                User = user,
                UserId = UserGuid,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-additional-info", userInfo.Convert(false), AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.Equal(HttpStatusCode.NotFound, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateAdditionalInfoButUserIsNotExist_NotFound()
        {
            // Arrange
            await InitializeUserDependency(UserGuid, "bot");

            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == "bot");

            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == UserGuid);

            UserAdditionalInfo userInfo = new()
            {
                Id = user!.AdditionalInfo!.Id,
                IsConfirmed = true,
                Balance = 9999,
                Role = userRole!,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-additional-info", userInfo.Convert(false), AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.Equal(HttpStatusCode.NotFound, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateAdditionalInfoButInfoNotExist_NotFound()
        {
            // Arrange
            await InitializeUserDependency(UserGuid, "bot");

            UserRole? userRole = await Context.UserRoles
                .FirstOrDefaultAsync(x => x.Name == "bot");
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == UserGuid);

            UserAdditionalInfo userInfo = new()
            {
                IsConfirmed = true,
                Balance = 9999,
                Role = userRole!,
                RoleId = userRole!.Id,
                User = user,
                UserId = UserGuid,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-additional-info", userInfo.Convert(false), AccessToken);

            // Assert
            await RemoveUserDependency(UserGuid);
            Assert.Equal(HttpStatusCode.NotFound, putStatusCode);
        }
    }
}
