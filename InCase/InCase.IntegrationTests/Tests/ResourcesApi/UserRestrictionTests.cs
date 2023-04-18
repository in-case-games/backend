﻿using InCase.Domain.Common;
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
    public class UserRestrictionTests: BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();
        private readonly Dictionary<string, Guid> DependencyGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["Owner"] = Guid.NewGuid(),
            ["Slave"] = Guid.NewGuid(),
            ["Restriction"] = Guid.NewGuid()
        };
        public UserRestrictionTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Fact]
        public async Task GET_RestrictionById_OK()
        {
            // Arrange
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{DependencyGuids["Restriction"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionById_NotFound()
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByUserId_OK()
        {
            // Arrange
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/user/{DependencyGuids["Slave"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByUserId_NotFoundUser()
        {
            // Arrange
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/user/{DependencyGuids["Slave"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByUserId_NotFoundRestriction()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/user/{DependencyGuids["User"]}");

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByToken_Ok()
        {
            // Arrange
            await InitializeDependencies();
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Slave"]);
            string token = CreateToken(owner!);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-restriction", token);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByToken_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"]);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-restriction", AccessToken);

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestictionByToken_Unauthorized()
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
               .ResponseGetStatusCode("/api/user-restriction");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, getStatusCode);
        }

        [Fact]
        public async Task GET_RestrictionByIds_Ok()
        {
            // Arrange
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{DependencyGuids["Owner"]}&{DependencyGuids["Slave"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionById_NotFoundUser()
        {
            // Arrange
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{DependencyGuids["Owner"]}&{Guid.NewGuid()}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionById_NotFoundRestriction()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"]);
            await InitializeDependencies();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{DependencyGuids["Owner"]}&{DependencyGuids["User"]}");

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByIds_NotFound()
        {
            // Arrange
            await InitializeDependencies();
            UpdateContext();

            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(x => x.Id == DependencyGuids["Restriction"]);
            Context.UserRestrictions.Remove(restriction!);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/{DependencyGuids["Owner"]}&{DependencyGuids["Slave"]}");

            // Assert
            UpdateContext();
            User? owner = await Context.Users.FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            User? slave = await Context.Users.FirstOrDefaultAsync(f => f.Id == DependencyGuids["Slave"]);
           
            Context.Users.RemoveRange(owner!, slave!);

            await Context.SaveChangesAsync();

            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByAdmin_Ok()
        {
            // Arrange
            await InitializeDependencies();
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-restriction/owner", token);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByAdminAndUserId_Ok()
        {
            // Arrange
            await InitializeDependencies();
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/owner/{DependencyGuids["Slave"]}", token);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_RestrictionByAdmin_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"], "admin");

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-restriction/owner", AccessToken);

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Theory]
        [InlineData("/api/user-restriction/owner", Roles.User)]
        [InlineData("/api/user-restriction/owner/6F9619FF-8B86-D011-B42D-00CF4FC964FF", Roles.User)]
        public async Task GET_RestrictionByAdmin_Forbidden(string uri, string roleName)
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"], roleName);

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode(uri, AccessToken);

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            Assert.Equal(HttpStatusCode.Forbidden, getStatusCode);
        }
        #region Зависимости
        private async Task InitializeDependencies()
        {
            UpdateContext();
            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == "bot");

            UserAdditionalInfo ownerInfo = new()
            {
                IsConfirmed = true,
                Balance = 1234,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            User owner = new()
            {
                Id = DependencyGuids["Owner"],
                Login = $"{GenerateString()}RestrictionApiTest",
                Email = $"{GenerateString()}@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1",
                AdditionalInfo = ownerInfo,
            };


            await Context.Users.AddAsync(owner);
            await Context.UserAdditionalInfos.AddAsync(ownerInfo);
            await Context.SaveChangesAsync();

            UpdateContext();

            UserAdditionalInfo slaveInfo = new()
            {
                IsConfirmed = true,
                Balance = 1234,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };
            User slave = new()
            {
                Id = DependencyGuids["Slave"],
                Login = $"{GenerateString()}RestrictionApiTest",
                Email = $"{GenerateString()}@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1",
                AdditionalInfo = slaveInfo,
            };

            RestrictionType? restrictionType = await Context.RestrictionTypes.FirstOrDefaultAsync(f => f.Name == "warn");

            UserRestriction restriction = new()
            {
                Id = DependencyGuids["Restriction"],
                CreationDate = DateTime.UtcNow,
                Description = $"{GenerateString(8)}RestrictionDescription",
                OwnerId = DependencyGuids["Owner"],
                UserId = DependencyGuids["Slave"],
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(2),
                Type = restrictionType,
                TypeId = restrictionType!.Id
            };

            await Context.Users.AddAsync(slave);
            await Context.UserAdditionalInfos.AddAsync(slaveInfo);
            await Context.UserRestrictions.AddAsync(restriction);
            await Context.SaveChangesAsync();
        }
        private async Task RemoveDependencies()
        {
            UpdateContext();
            User? owner = await Context.Users.FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            User? slave = await Context.Users.FirstOrDefaultAsync(f => f.Id == DependencyGuids["Slave"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            Context.UserRestrictions.Remove(restriction!);
            Context.Users.RemoveRange(owner!, slave!);

            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
