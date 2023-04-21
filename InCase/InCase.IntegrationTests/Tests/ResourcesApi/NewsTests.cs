using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
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
        [Fact]
        public async Task POST_News_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            Context.News.Remove(news);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news", news, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_News_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            Context.News.Remove(news);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news", news);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task POST_News_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            Context.News.Remove(news);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news", news, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_NewsImage_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            NewsImage image = await Context.NewsImages
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["NewsImage"]);

            Context.NewsImages.Remove(image);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news/image", image.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_NewsImage_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            NewsImage image = await Context.NewsImages
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["NewsImage"]);

            Context.NewsImages.Remove(image);
            await Context.SaveChangesAsync();

            image.News = null;
            image.NewsId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news/image", image.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_NewsImage_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            NewsImage image = await Context.NewsImages
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["NewsImage"]);

            Context.NewsImages.Remove(image);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news/image", image.Convert());

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task POST_NewsImage_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            NewsImage image = await Context.NewsImages
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["NewsImage"]);

            Context.NewsImages.Remove(image);
            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/news/image", image.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task PUT_News_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/news", news, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_News_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            news.Id = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/news", news, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_News_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/news", news);

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task PUT_News_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            News news = await Context.News
                .AsNoTracking()
                .FirstAsync(f => f.Id == DependenciesGuids["News"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/news", news, AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_News_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/{DependenciesGuids["News"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_News_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/{DependenciesGuids["News"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_News_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/{DependenciesGuids["News"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task DELETE_News_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/{DependenciesGuids["News"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_NewsImage_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/image/{DependenciesGuids["NewsImage"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_NewsImage_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/image/{DependenciesGuids["NewsImage"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_NewsImage_Unauthorized()
        {
            //Arrange
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/image/{DependenciesGuids["NewsImage"]}");

            //Assert
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task DELETE_NewsImage_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/news/image/{DependenciesGuids["NewsImage"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
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
