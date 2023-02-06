using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeUsedByUserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private Promocode Promocode { get; set; } = new();

        public PromocodeUsedByUserApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }

        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULPUBUST{ipUser}User",
                UserEmail = $"UEPUBUST{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULPUBUST{ipAdmin}Admin",
                UserEmail = $"UEPUBUST{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }
        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULPUBUST{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULPUBUST{ipAdmin}Admin");
        }

        private async Task InitializePromocode()
        {
            Guid searchTypePromocodeId = (await _response.ResponseGet<PromocodeType>(
                "/PromocodeType/GetByName?name=balance"))!
                .Id;

            Promocode = new()
            {
                PromocodeDiscount = 10M,
                PromocodeName = "PNPUBUSTName",
                PromocodeTypeId = searchTypePromocodeId,
                PromocodeUsesCount = 1000000000
            };

            await _response.ResponsePostStatusCode("/Promocode", Promocode, AdminTokens.AccessToken!);
            Promocode.Id = (await _response.ResponseGet<Promocode>(
                $"/Promocode/GetByName?" +
                $"name={Promocode.PromocodeName}", AdminTokens.AccessToken!))!.Id;
        }

        [Fact]
        public async Task PromocodeUsedByUserSimpleTest()
        {
            await InitializeOneTimeAccounts("0.7.0", "0.7.1");

            // Arrange
            await InitializePromocode();

            User.Id = (await _response.ResponseGet<User>(
                $"/User/GetByLogin?login={User.UserLogin}", UserTokens.AccessToken!))!.Id;

            // Act
            HttpStatusCode postStatusCode = await _response.ResponsePostStatusCode(
                $"/Promocode/UsePromocode", Promocode, UserTokens.AccessToken!);

            List<PromocodesUsedByUser> promocodesUsedByUser = (await _response
                .ResponseGet<List<PromocodesUsedByUser>>
                ($"/PromocodesUsedByUser/GetAll?userId={User.Id}"))!;
            PromocodesUsedByUser promocodeUsedByUser = promocodesUsedByUser
                .FirstOrDefault(x => x.PromocodeId == Promocode.Id)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode(
                $"/PromocodesUsedByUser?" +
                $"id={promocodeUsedByUser.Id!}", 
                UserTokens.AccessToken!);

            await _response.ResponseDelete(
                $"/Promocode?" +
                $"id={promocodeUsedByUser.PromocodeId}", 
                AdminTokens.AccessToken!);

            await DeleteOneTimeAccounts("0.7.0", "0.7.1");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode));
        }
    }
}
