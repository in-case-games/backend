using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.IntegrationTests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace InCase.IntegrationTests.Tests
{
    public class BaseApiTest
    {
        protected readonly ITestOutputHelper _output;
        protected string AccessToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjhjMTgzYjc2LTY4MGMtNDY1MC04ZTY2LWRlMTAyZDE3MGRkNCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im93bmVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkdJUyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Inl0X2ZlcmJyYXlAbWFpbC5ydSIsImV4cCI6MTY4MTY1ODIzMywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdCJ9.I0RAdC4Wm-MHyhnzn0ERDAzUvORzF6R-ByTNNY-dnaalLjX0yTKw8zuo0bNw1nxy1v68zfEXm7AGb83oz-QduQ";
        protected string RefreshToken = "";
        protected ApplicationDbContext Context { get; }

        public BaseApiTest(
            ITestOutputHelper output,
            IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention()
                .UseSqlServer(configuration["ConnectionStrings:TestingConnection"])
                .Options;

            Context = new ApplicationDbContext(options);
            Context.Database.Migrate();

            _output = output;
        }
    }
}
