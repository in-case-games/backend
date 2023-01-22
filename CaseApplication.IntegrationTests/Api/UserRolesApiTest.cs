using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRolesApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserRolesApiTest(WebApplicationFactory<Program> application) 
        {
            _response = new ResponseHelper(application.CreateClient());
        }

    }
}
