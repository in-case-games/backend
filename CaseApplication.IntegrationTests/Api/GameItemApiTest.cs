using CaseApplication.WebClient.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class GameItemApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ClientApiRepository _clientApi;
        public GameItemApiTest(WebApplicationFactory<Program> application)
        {
            _clientApi = new(application.CreateClient());
        }
        [Fact]
        public async Task GetItemTest()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
