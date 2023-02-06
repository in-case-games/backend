using Azure;
using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper;
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private Promocode Promocode { get; set; } = new();

        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULPST{ipUser}User",
                UserEmail = $"ULPST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULPST{ipAdmin}Admin",
                UserEmail = $"UEPST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULPST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULPST{ipAdmin}Admin");
        }

        public PromocodeApiTest(WebApplicationFactory<Program> applicationFactory) 
        {
            _clientApi = new(applicationFactory.CreateClient());
            _authHelper = new AuthenticationTestHelper(_clientApi);
        }

        [Fact]
        public async Task PromocodeSimpleTests()
        {
            await InitializeOneTimeAccounts("0.5.0", "0.5.1");

            //Create
            Guid searchTypePromocodeId = (await _clientApi.ResponseGet<PromocodeType>(
                "/PromocodeType/GetByName?" +
                "name=balance"))!
                .Id;

            Promocode = new()
            {
                PromocodeDiscount = 10M,
                PromocodeName = "Стандарт на пополнение",
                PromocodeTypeId = searchTypePromocodeId,
                PromocodeUsesCount = 1000000000
            };

            await _clientApi.ResponsePostStatusCode(
                "/Promocode", Promocode, AdminTokens.AccessToken!);

            //Get
            Promocode.Id = await GetById();

            //Update
            Promocode.PromocodeUsesCount = 999999999;
            await _clientApi.ResponsePut(
                "/Promocode", Promocode, AdminTokens.AccessToken!);

            //Delete
            await _clientApi.ResponseDelete(
                $"/Promocode?id={Promocode.Id}", AdminTokens.AccessToken!);

            await DeleteOneTimeAccounts("0.5.0", "0.5.1");
        }

        private async Task<Guid> GetById()
        {
            Promocode? promocode = await _clientApi.ResponseGet<Promocode>(
                $"/Promocode/GetByName?" +
                $"name={Promocode.PromocodeName!}", AdminTokens.AccessToken!);

            if (promocode == null) throw new Exception("No such promocode");

            return promocode.Id;
        }
    }
}
