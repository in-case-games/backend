using InCase.Infrastructure.Data;
using InCase.IntegrationTests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace InCase.IntegrationTests.Tests
{
    public class BaseApiTest
    {
        protected readonly ITestOutputHelper _output;
        protected string AccessToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImUwNGQzOGE5LWVmNWYtNDYzYS05NDQyLWY1ZTE1ZmMxOWI3YiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im93bmVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkdJUyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Inl0X2ZlcmJyYXlAbWFpbC5ydSIsImV4cCI6MTY4MTU4NjYzMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdCJ9.UVakhmmowh6e4dpogElooH8MuPOSjj4ripg30SBbnR6YpGdNFSExfHi8Zkx5gYTuIAPrHuRXWL_zbAeybwzrLw";
        protected string RefreshToken = "";
        protected ApplicationDbContext Context { get; }

        public BaseApiTest(
            ITestOutputHelper output,
            IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention()
                .UseSqlServer(configuration["ConnectionStrings:DevelopmentConnection"])
                .Options;

            Context = new ApplicationDbContext(options);
            _output = output;
        }
    }
}
