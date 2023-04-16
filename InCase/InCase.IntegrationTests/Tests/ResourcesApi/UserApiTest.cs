using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class UserApiTest: BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostGameApiTests>()
            .Build();

        public UserApiTest(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output): base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Fact]
        public async Task GET_GetAllUsers_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            await InitializeDependency(id);
        }

        #region Начальные данные
        private async Task InitializeDependency(Guid guid, string roleName = "user")
        {
            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == roleName);

            UserAdditionalInfo userInfo = new()
            {
                IsConfirmed = true,
                Balance = 99999999M,
                Role = userRole!,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };
            User user = new()
            {
                Id = guid,
                Login = "UserUserForTests1",
                Email = "sex@mail.ru",
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
