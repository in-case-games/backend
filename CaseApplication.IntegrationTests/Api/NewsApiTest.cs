using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;

namespace CaseApplication.IntegrationTests.Api
{
    public class NewsApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;

        public NewsApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());
        }

        [Fact]
        public async Task NewsSimpleApiTest()
        {
            bool IsCreated = await CreateNews();

            News news = await GetNewsByName("NSATName");
            news.NewsDate = DateTime.Now;

            bool IsUpdated = await UpdateNews(news);
            bool IsDeleted = await DeleteNews(news.Id);
            bool IsComplite = (IsCreated) && (IsUpdated) && (IsDeleted);

            Assert.True(IsComplite);
        }

        private async Task<News> GetNewsByName(string name)
        {
            List<News> allNews = await _clientApi.ResponseGet<List<News>>("/News/GetAll");
            return allNews.FirstOrDefault(x => x.NewsName == name)!;
        }

        private async Task<bool> CreateNews()
        {
            News news = new()
            {
                NewsName = "NSATName",
                NewsContent = "NSATContent",
                NewsDate = DateTime.Now,
                NewsImage = "NSATImage"
            };

            return HttpStatusCode.OK == await _clientApi.ResponsePost<News>("/News", news);
        }

        private async Task<bool> UpdateNews(News news)
        {
            return HttpStatusCode.OK == await _clientApi.ResponsePut<News>("/News", news);
        }

        private async Task<bool> DeleteNews(Guid id)
        {
            return HttpStatusCode.OK == await _clientApi.ResponseDelete($"/News?id={id}");
        }
    }
}
