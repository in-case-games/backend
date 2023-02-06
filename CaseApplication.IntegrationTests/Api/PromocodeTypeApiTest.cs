using Azure;
using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeTypeApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper;
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();

        public PromocodeTypeApiTest(WebApplicationFactory<Program> applicationBuilder)
        {
            _clientApi = new(applicationBuilder.CreateClient());
            _authHelper = new AuthenticationTestHelper(_clientApi);
        }
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULPTST{ipUser}User",
                UserEmail = $"ULPTST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULPTST{ipAdmin}Admin",
                UserEmail = $"UEPTST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULPTST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULPTST{ipAdmin}Admin");
        }

        [Fact]
        public async Task PromocodeTypeSimpleTests()
        {
            await InitializeOneTimeAccounts("0.6.0", "0.6.1");

            //Create
            PromocodeType promocodeType = new()
            {
                PromocodeTypeName = "Test"
            };

            await _clientApi.ResponsePostStatusCode(
                "/PromocodeType", promocodeType, AdminTokens.AccessToken!);

            //Get and GetAll
            promocodeType.Id = await GetIdByName(promocodeType.PromocodeTypeName);

            //Update
            promocodeType.PromocodeTypeName = "Test2";
            await _clientApi.ResponsePut(
                "/PromocodeType", promocodeType, AdminTokens.AccessToken!);

            //Delete
            await _clientApi.ResponseDelete(
                $"/Promocode?id={promocodeType.Id}", 
                AdminTokens.AccessToken!);

            await DeleteOneTimeAccounts("0.6.0", "0.6.1");
        }

        private async Task<Guid> GetIdByName(string name)
        {
            PromocodeType? type = await _clientApi.ResponseGet<PromocodeType>(
                $"/PromocodeType/GetByName?name={name}", AdminTokens.AccessToken!);

            if (type == null) throw new Exception("No such type promocode");

            return type.Id;
        }
    }
}
