﻿using InCase.Domain.Common;
using InCase.Domain.Dtos;
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
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
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_GetRestrictionByAdminAndUserId_OK()
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
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task GET_OwnerRestrictionByUserId_NotFoundUser()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"], "bot");

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-restriction/owner/{DependencyGuids["Slave"]}", AccessToken);

            // Assert
            await RemoveUserDependency(DependencyGuids["User"]);
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_OwnerRestrictionByUserId_NotFoundRestrictions()
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
                .ResponseGetStatusCode($"/api/user-restriction/owner/{DependencyGuids["User"]}", token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Fact]
        public async Task GET_OwnerRestriction_NotFound()
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
        [Fact]
        public async Task GET_RestrictionTypes_OK()
        {
            // Assert

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-restriction/types");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getStatusCode);
        }
        [Fact]
        public async Task POST_Restriction_OK()
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], "bot");
            await CreateUser(DependencyGuids["Slave"], "user");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

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

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false), token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.OK, postStatusCode);
        }
        [Theory]
        [InlineData("user")]
        public async Task POST_Restriction_Forbidden(string roleName)
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], roleName);
            await CreateUser(DependencyGuids["Slave"], "user");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

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

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false), token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Forbidden, postStatusCode);
        }
        [Fact]
        public async Task POST_Restriction_NotFoundType()
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], "bot");
            await CreateUser(DependencyGuids["Slave"], "user");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

            RestrictionType? restrictionType = await Context.RestrictionTypes.FirstOrDefaultAsync(f => f.Name == "warn");

            UserRestriction restriction = new()
            {
                Id = DependencyGuids["Restriction"],
                CreationDate = DateTime.UtcNow,
                Description = $"{GenerateString(8)}RestrictionDescription",
                OwnerId = DependencyGuids["Owner"],
                UserId = DependencyGuids["Slave"],
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(2),
                TypeId = DependencyGuids["Slave"]
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false), token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, postStatusCode);
        }
        [Fact]
        public async Task POST_Restriction_Unauthorized()
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], "bot");
            await CreateUser(DependencyGuids["Slave"], "user");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

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

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false));

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Unauthorized, postStatusCode);
        }
        [Fact]
        public async Task POST_Restriction_NotFoundUser()
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], "bot");
            await CreateUser(DependencyGuids["Slave"], "user");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

            RestrictionType? restrictionType = await Context.RestrictionTypes.FirstOrDefaultAsync(f => f.Name == "warn");

            UserRestriction restriction = new()
            {
                Id = DependencyGuids["Restriction"],
                CreationDate = DateTime.UtcNow,
                Description = $"{GenerateString(8)}RestrictionDescription",
                OwnerId = DependencyGuids["Owner"],
                UserId = DependencyGuids["User"],
                ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(2),
                Type = restrictionType,
                TypeId = restrictionType!.Id
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false), token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, postStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestriction_OK()
        {
            // Arrange
            await InitializeDependencies();
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false), token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.OK, updateStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestriction_NotFoundType()
        {
            // Arrange
            await InitializeDependencies();
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            restriction!.Type = null;
            restriction!.TypeId = Guid.NewGuid();

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false), token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, updateStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestriction_NotFoundRestriction()
        {
            // Arrange
            await InitializeDependencies();
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            restriction!.Id = Guid.NewGuid();

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false), token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, updateStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestriction_NotFoundUser()
        {
            // Arrange
            await InitializeDependencies();
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            UserRestrictionDto sendRestriction = restriction!.Convert(false);

            sendRestriction.UserId = Guid.NewGuid();

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", sendRestriction, token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.NotFound, updateStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestrictionNotAccessUser_Conflict()
        {
            // Arrange
            await InitializeDependencies("bot", "admin");
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false), token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Conflict, updateStatusCode);
        }
        [Fact]
        public async Task PUT_UserRestriction_Unauthorized()
        {
            // Arrange
            await InitializeDependencies();
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false));

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Unauthorized, updateStatusCode);
        }
        [Theory]
        [InlineData("user")]
        public async Task PUT_UserRestriction_Forbidden(string roleName)
        {
            // Arrange
            await InitializeDependencies(ownerRoleName: roleName, slaveRoleName: "bot");
            User? user = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(y => y!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            string token = CreateToken(user!);

            // Act
            HttpStatusCode updateStatusCode = await _responseService
                .ResponsePut<UserRestrictionDto>("/api/user-restriction", restriction!.Convert(false), token);

            // Assert 
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Forbidden, updateStatusCode);
        }
        [Fact]
        public async Task POST_Restriction_ConflictInvalidRole()
        {
            // Assert
            await CreateUser(DependencyGuids["Owner"], "bot");
            await CreateUser(DependencyGuids["Slave"], "bot");
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

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

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-restriction", restriction.Convert(false), token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Conflict, postStatusCode);
        }
        [Fact]
        public async Task DELETE_RemoveRestriction_OK()
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
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-restriction/{DependencyGuids["Restriction"]}", token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_RemoveRestriction_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"], "bot");

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-restriction/{DependencyGuids["Restriction"]}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_RemoveRestriction_Conflict()
        {
            // Arrange
            await InitializeDependencies();
            UpdateContext();
            User? owner = await Context.Users
                .Include(x => x.AdditionalInfo)
                .ThenInclude(x => x!.Role)
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Owner"]);
            string token = CreateToken(owner!);

            UpdateContext();
            UserRestriction? restriction = await Context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == DependencyGuids["Restriction"]);

            restriction!.UserId = DependencyGuids["Owner"];
            Context.UserRestrictions.Update(restriction);
            await Context.SaveChangesAsync();

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-restriction/{DependencyGuids["Restriction"]}", token);

            // Assert
            await ClearTableData("UserRestriction", "User");
            Assert.Equal(HttpStatusCode.Conflict, deleteStatusCode);
        }
        [Theory]
        [InlineData("user")]
        public async Task DELETE_RemoveRestriction_Forbidden(string roleName)
        {
            // Arrange
            await InitializeUserDependency(DependencyGuids["User"], roleName);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-restriction/{DependencyGuids["Restriction"]}", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, deleteStatusCode);
        }
        #region Зависимости
        private async Task InitializeDependencies(string ownerRoleName = "bot", string slaveRoleName = "user")
        {
            UpdateContext();

            await CreateUser(DependencyGuids["Owner"], ownerRoleName);
            await CreateUser(DependencyGuids["Slave"], slaveRoleName);

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

            await Context.UserRestrictions.AddAsync(restriction);
            await Context.SaveChangesAsync();
        }
        private async Task CreateUser(Guid guid, string roleName)
        {
            UpdateContext();
            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == roleName);

            UserAdditionalInfo userInfo = new()
            {
                IsConfirmed = true,
                Balance = 1234,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };

            User user = new()
            {
                Id = guid,
                Login = $"{GenerateString()}RestrictionApiTest",
                Email = $"{GenerateString()}@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1",
                AdditionalInfo = userInfo,
            };


            await Context.Users.AddAsync(user);
            await Context.UserAdditionalInfos.AddAsync(userInfo);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
