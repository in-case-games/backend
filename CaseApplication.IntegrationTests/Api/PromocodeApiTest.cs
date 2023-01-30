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

        }
    }
}
