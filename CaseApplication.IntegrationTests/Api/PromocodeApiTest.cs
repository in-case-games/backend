using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private ResponseHelper _clientApi;

        public PromocodeApiTest(WebApplicationFactory<Program> applicationFactory) 
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task PromocodeSimpleTests()
        {
            //Create
            Guid searchTypePromocodeId = (await _clientApi
                .ResponseGet<PromocodeType>("/PromocodeType/GetByName?name=balance")).Id;

            Promocode promocode = new() {
                PromocodeDiscount = 10M,
                PromocodeName = "Стандарт на пополнение",
                PromocodeTypeId = searchTypePromocodeId, 
                PromocodeUsesCount = 1000000000
            };

            await _clientApi.ResponsePost("/Promocode", promocode);

            //Get
            promocode = await _clientApi
                .ResponseGet<Promocode>($"/Promocode/GetByName?name={promocode.PromocodeName}");

            //Update
            promocode.PromocodeUsesCount = 999999999;
            await _clientApi.ResponsePut("/Promocode", promocode);

            //Delete
            await _clientApi.ResponseDelete($"/Promocode?id={promocode.Id}");
        }
    }
}
