using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeUsedByUserApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public PromocodeUsedByUserApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task<PromocodesUsedByUser> InitializePromocodeUsedByUser()
        {
            User user = InitializeUser();
            await _response.ResponsePost("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User/GetByLogin?login={user.UserLogin}&hash=123");

            Promocode promocode = await InitializePromocode();
            await _response.ResponsePost("/Promocode", promocode);
            Promocode currentPromocode = await _response
                .ResponseGet<Promocode>($"/Promocode/GetByName?name=Стандарт на пополнение");

            PromocodesUsedByUser promocodesUsedByUser = new PromocodesUsedByUser
            {
                PromocodeId = currentPromocode.Id,
                UserId = currentUser.Id
            };

            return promocodesUsedByUser;

        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserLogin = "testlogin1",
                UserEmail = "testemail1",
                UserImage = "testimage1",
                PasswordHash = "test1",
                PasswordSalt = "test1",
            };

            return user;
        }
        public async Task<Promocode> InitializePromocode()
        {
            Guid searchTypePromocodeId = (await _response
                .ResponseGet<PromocodeType>("/PromocodeType/GetByName?name=balance")).Id;

            Promocode promocode = new()
            {
                PromocodeDiscount = 10M,
                PromocodeName = "Стандарт на пополнение",
                PromocodeTypeId = searchTypePromocodeId,
                PromocodeUsesCount = 1000000000
            };

            return promocode;
        }
        [Fact]
        public async Task PromocodeUsedByUserCrudTest()
        {
            // Arrange
            PromocodesUsedByUser tempPromocodesUsedByUser = await InitializePromocodeUsedByUser();

            // Act
            HttpStatusCode postStatusCode = await _response
                .ResponsePost("/PromocodesUsedByUser", tempPromocodesUsedByUser);

            IEnumerable<PromocodesUsedByUser> promocodesUsedByUser = await _response
                .ResponseGet<List<PromocodesUsedByUser>>
                ($"/PromocodesUsedByUser/GetAll?userId={tempPromocodesUsedByUser.UserId}");
            PromocodesUsedByUser promocodeUsedByUser = promocodesUsedByUser
                .FirstOrDefault(x => x.UserId == tempPromocodesUsedByUser.UserId
                && x.PromocodeId == tempPromocodesUsedByUser.PromocodeId)!;

            HttpStatusCode getStatusCode = await _response
                .ResponseGetStatusCode($"/PromocodesUsedByUser?id={promocodeUsedByUser.Id!}");
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/PromocodesUsedByUser/GetAll?userId={tempPromocodesUsedByUser.UserId}");

            HttpStatusCode putStatusCode = await _response
                .ResponsePut("/PromocodesUsedByUser", promocodeUsedByUser);

            HttpStatusCode deleteStatusCode = await _response
                .ResponseDelete($"/PromocodesUsedByUser?id={promocodeUsedByUser.Id}");

            await _response.ResponseDelete($"/User?id={promocodeUsedByUser.UserId}");
            await _response.ResponseDelete($"/Promocode?id={promocodeUsedByUser.PromocodeId}");

            // Assert
            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK),
                (postStatusCode, getStatusCode, getAllStatusCode, putStatusCode, deleteStatusCode));
        }
    }
}
