using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class NewsApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _clientApi;
        private readonly AuthenticationTestHelper _authHelper = new();
        private TokenModel UserTokens { get; set; } = new();
        private TokenModel AdminTokens { get; set; } = new();
        private User User { get; set; } = new();
        private User Admin { get; set; } = new();
        private News News { get; set; } = new();
        public NewsApiTest(WebApplicationFactory<Program> applicationFactory)
        {
            _clientApi = new(applicationFactory.CreateClient());

            News = new()
            {
                NewsName = "NSATName",
                NewsContent = "NSATContent",
                NewsDate = DateTime.UtcNow,
                NewsImage = "NSATImage"
            };
        }
        
        private async Task InitializeOneTimeAccounts(string ipUser, string ipAdmin)
        {
            User = new()
            {
                UserLogin = $"ULNSAT{ipUser}User",
                UserEmail = $"ULNSAT{ipUser}User"
            };
            Admin = new()
            {
                UserLogin = $"ULNSAT{ipAdmin}Admin",
                UserEmail = $"ULNSAT{ipAdmin}Admin"
            };

            UserTokens = await _authHelper.SignInUser(User, ipUser);
            AdminTokens = await _authHelper.SignInAdmin(Admin, ipAdmin);
        }

        private async Task DeleteOneTimeAccounts(string ipUser, string ipAdmin)
        {
            await _authHelper.DeleteUserByAdmin($"ULNSAT{ipUser}User");
            await _authHelper.DeleteUserByAdmin($"ULNSAT{ipAdmin}Admin");
        }

        [Fact]
        public async Task NewsSimpleApiTest()
        {
            await InitializeOneTimeAccounts("0.4.0", "0.4.1");

            HttpStatusCode codeCreated = await _clientApi.ResponsePostStatusCode(
                "/News", News, AdminTokens.AccessToken!);

            News.Id = await GetNewsId();
            News.NewsDate = DateTime.Now;

            bool IsUpdated = await UpdateNews();
            bool IsDeleted = await DeleteNews();
            bool IsComplite = 
                (codeCreated == HttpStatusCode.OK) && 
                (IsUpdated) && 
                (IsDeleted);

            await DeleteOneTimeAccounts("0.4.0", "0.4.1");

            Assert.True(IsComplite);
        }

        private async Task<Guid> GetNewsId()
        {
            List<News> allNews = await _clientApi.ResponseGet<List<News>>("/News/GetAll") ?? new();
            return allNews.FirstOrDefault(x => x.NewsName == News.NewsName)!.Id;
        }

        private async Task<bool> UpdateNews()
        {
            return HttpStatusCode.OK == await _clientApi.ResponsePut(
                "/News", News, AdminTokens.AccessToken!);
        }

        private async Task<bool> DeleteNews()
        {
            return HttpStatusCode.OK == await _clientApi.ResponseDelete(
                $"/News?id={News.Id}", AdminTokens.AccessToken!);
        }
    }
}
