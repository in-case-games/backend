using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class PromocodeTypeApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;

        public PromocodeTypeApiTest(WebApplicationFactory<Program> applicationBuilder)
        {
            _clientApi = new(applicationBuilder.CreateClient());
        }

        [Fact]
        public async Task PromocodeTypeSimpleTests()
        {
            //Create
            PromocodeType promocodeType = new()
            {
                PromocodeTypeName = "Test"
            };
            await _clientApi.ResponsePost("/PromocodeType", promocodeType);

            //Get and GetAll
            List<PromocodeType> promocodeTypes = await _clientApi
                .ResponseGet<List<PromocodeType>>("/PromocodeType/GetAll");

            promocodeType = await _clientApi
                .ResponseGet<PromocodeType>("/PromocodeType/GetByName?name=Test");

            //Update
            promocodeType.PromocodeTypeName = "Test2";
            await _clientApi.ResponsePut("/PromocodeType", promocodeType);

            //Delete
            await _clientApi.ResponseDelete($"/Promocode?id={promocodeType.Id}");
        }
    }
}
