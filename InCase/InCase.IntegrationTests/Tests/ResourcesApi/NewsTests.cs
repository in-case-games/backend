using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class NewsTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["News"] = Guid.NewGuid(),
            ["NewsImage"] = Guid.NewGuid()
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public NewsTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Fact]
        public async Task GET_News_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/news");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_News_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/news");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_NewsById_OK()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/news/{DependenciesGuids["News"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_NewsById_NotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/news/{DependenciesGuids["News"]}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            UpdateContext();

            News news = new()
            {
                Id = DependenciesGuids["News"],
                Title = "AAA",
                Content = "BBB",
                Date = DateTime.UtcNow
            };

            NewsImage image = new()
            {
                Id = DependenciesGuids["NewsImage"],
                ImageUri = "CCC",
                NewsId = news.Id
            };

            await Context.News.AddAsync(news);
            await Context.NewsImages.AddAsync(image);
            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<News> news = await Context.News
                .AsNoTracking()
                .ToListAsync();
            List<NewsImage> images = await Context.NewsImages
                .AsNoTracking()
                .ToListAsync();

            foreach (var oneNews in news)
                Context.News.Remove(oneNews);
            foreach (var image in images)
                Context.NewsImages.Remove(image);

            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
