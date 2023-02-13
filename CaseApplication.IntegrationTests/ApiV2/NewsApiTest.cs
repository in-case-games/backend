using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class NewsApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public NewsApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetNews_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            News news = new()
            {
                NewsName = "NEW UPDATE",
                NewsContent = "YRA YRA"
            };

            //Act
            await _response.ResponsePostStatusCode("/News/admin", news, AccessToken);
            List<News> listNews = (await _response.ResponseGet<List<News>>("/News/all"))!;
            HttpStatusCode statusCodeGet = await _response.ResponseGetStatusCode(
                $"/News/{listNews[0].Id}");
            await _response.ResponseDelete($"/News/admin/{listNews[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetAllNews_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            News news = new()
            {
                NewsName = "NEW UPDATE",
                NewsContent = "YRA YRA"
            };

            //Act
            await _response.ResponsePostStatusCode("/News/admin", news, AccessToken);
            List<News> listNews = (await _response.ResponseGet<List<News>>("/News/all"))!;
            HttpStatusCode statusCodeGet = listNews.Count > 0 ? 
                HttpStatusCode.OK : HttpStatusCode.NotFound;
            await _response.ResponseDelete($"/News/admin/{listNews[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task POST_CreateNews_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            News news = new()
            {
                NewsName = "NEW UPDATE",
                NewsContent = "YRA YRA"
            };

            //Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/News/admin", news, AccessToken);
            List<News> listNews = (await _response.ResponseGet<List<News>>("/News/all"))!;
            await _response.ResponseDelete($"/News/admin/{listNews[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateNews_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            News news = new()
            {
                NewsName = "NEW UPDATE",
                NewsContent = "YRA YRA"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/News/admin", news, AccessToken);
            List<News> listNews = (await _response.ResponseGet<List<News>>("/News/all"))!;
            listNews[0].NewsName = "NEW UPDATES";
            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                "/News/admin", listNews[0], AccessToken);
            await _response.ResponseDelete($"/News/admin/{listNews[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task DELETE_DeleteNews_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            News news = new()
            {
                NewsName = "NEW UPDATE",
                NewsContent = "YRA YRA"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/News/admin", news, AccessToken);
            List<News> listNews = (await _response.ResponseGet<List<News>>("/News/all"))!;
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/News/admin/{listNews[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
