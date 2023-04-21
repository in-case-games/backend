using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class SupportTopicTests : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["Support"] = Guid.NewGuid(),
            ["SupportTopic"] = Guid.NewGuid(),
            ["AnswerUser"] = Guid.NewGuid(),
            ["AnswerSupport"] = Guid.NewGuid(),
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public SupportTopicTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient(), "https://localhost:7102");
        }

        [Fact]
        public async Task GET_UserTopics_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_UserTopics_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopics_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic");

            //Assert

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_UserTopics_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicById_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicById_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicById_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}");

            //Assert

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_UserTopicById_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswers_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswers_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            Guid randomUserGuid = Guid.NewGuid();
            await InitializeUserDependency(randomUserGuid, "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveUserDependency(randomUserGuid);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswers_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswers_NotFoundAnswers()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            List<SupportTopicAnswer> answers = await Context.SupportTopicAnswers
                .AsNoTracking()
                .ToListAsync();

            foreach(var answer in answers)
                Context.SupportTopicAnswers.Remove(answer);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswers_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_UserTopicAnswers_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            Guid randomUserGuid = Guid.NewGuid();
            await InitializeUserDependency(randomUserGuid, "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveUserDependency(randomUserGuid);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_NotFoundAnswerId()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{Guid.NewGuid()}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_NotFoundAnswers()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            List<SupportTopicAnswer> answers = await Context.SupportTopicAnswers
                .AsNoTracking()
                .ToListAsync();

            foreach (var answer in answers)
                Context.SupportTopicAnswers.Remove(answer);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_UserTopicAnswer_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task GET_UserTopicAnswer_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopics_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopics_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopics_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopics_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicClose_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/close", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicClose_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/close",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicClose_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/close");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task GET_SupportTopicClose_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/close",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicById_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicById_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicById_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicById_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicAnswerById_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}", AccessToken);

            HttpStatusCode statusCode2 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerSupport"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK), 
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task GET_SupportTopicAnswerById_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}", AccessToken);

            HttpStatusCode statusCode2 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerSupport"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(
                (HttpStatusCode.NotFound, HttpStatusCode.NotFound),
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task GET_SupportTopicAnswerById_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}");

            HttpStatusCode statusCode2 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerSupport"]}");

            //Assert
            Assert.Equal(
                (HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized),
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task GET_SupportTopicAnswerById_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}", AccessToken);

            HttpStatusCode statusCode2 = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerSupport"]}", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.Forbidden, HttpStatusCode.Forbidden),
                (statusCode1, statusCode2));
        }
        [Fact]
        public async Task GET_SupportTopicAnswersById_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicAnswersById_NotFoundAnswers()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            List<SupportTopicAnswer> answers = await Context.SupportTopicAnswers
                .AsNoTracking()
                .ToListAsync();

            foreach (var answer in answers)
                Context.SupportTopicAnswers.Remove(answer);

            await Context.SaveChangesAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicAnswersById_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support/answers",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicAnswersById_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support/answers");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task GET_SupportTopicAnswersById_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/support-topic/" +
                $"{DependenciesGuids["SupportTopic"]}" +
                $"/support/answers", AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_UserTopic_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopic topic = await Context.SupportTopics
                .AsNoTracking()
                .FirstAsync();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_UserTopic_Conflict()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopic topic = await Context.SupportTopics
                .AsNoTracking()
                .FirstAsync();

            await RemoveTestDependencies();

            //Act
            HttpStatusCode statusCode1 = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);
            HttpStatusCode statusCode2 = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);
            HttpStatusCode statusCode3 = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);
            HttpStatusCode statusCode4 = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(
                (HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.Conflict), 
                (statusCode1, statusCode2, statusCode3, statusCode4));
        }
        [Fact]
        public async Task POST_UserTopic_Unauthorized()
        {
            //Arrange
            SupportTopic topic = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task POST_UserTopic_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            SupportTopic topic = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic", topic.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_UserTopicAnswer_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/answer", answer.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_UserTopicAnswer_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            answer.TopicId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/answer", answer.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_UserTopicAnswer_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            Guid randomUserGuid = Guid.NewGuid();

            await InitializeUserDependency(randomUserGuid, "user");

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            answer.PlaintiffId = randomUserGuid;

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/answer", answer.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveUserDependency(randomUserGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_UserTopicAnswer_Unauthorized()
        {
            //Arrange
            SupportTopicAnswer answer = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/answer", answer.Convert());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task POST_UserTopicAnswer_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/answer", answer.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task POST_SupportTopicAnswer_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["Support"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/support/answer", 
                answer.Convert(), 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task POST_SupportTopicAnswer_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["Support"]);

            answer.TopicId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/support/answer",
                answer.Convert(),
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task POST_SupportTopicAnswer_Unauthorized()
        {
            //Arrange
            SupportTopicAnswer answer = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/support/answer", answer.Convert());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task POST_SupportTopicAnswer_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], role);
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["Support"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePostStatusCode($"/api/support-topic/support/answer",
                answer.Convert(),
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopic_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopic topic = await Context.SupportTopics
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic", topic.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopic_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopic topic = await Context.SupportTopics
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic", topic.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopic_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            Guid randomUserGuid = Guid.NewGuid();

            await InitializeUserDependency(randomUserGuid, "user");

            SupportTopic topic = await Context.SupportTopics
                .AsNoTracking()
                .FirstAsync();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic", topic.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveUserDependency(randomUserGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopic_Unauthorized()
        {
            //Arrange
            SupportTopic topic = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                 .ResponsePut($"/api/support-topic", topic.Convert(false));

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task PUT_UserTopic_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], role);

            SupportTopic topic = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic", topic.Convert(), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("owner")]
        public async Task PUT_UserSupportTopicAnswer_OK(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic/answer", answer.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopicAnswer_NotFoundTopic()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            answer.TopicId = Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic/answer", answer.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopicAnswer_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "admin");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            Guid randomUserGuid = Guid.NewGuid();

            await InitializeUserDependency(randomUserGuid, "user");

            SupportTopicAnswer answer = await Context.SupportTopicAnswers
                .AsNoTracking()
                .FirstAsync(f => f.PlaintiffId == DependenciesGuids["User"]);

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic/answer", answer.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveUserDependency(randomUserGuid);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopicAnswer_Unauthorized()
        {
            //Arrange
            SupportTopicAnswer answer = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic/answer", answer.Convert(false));

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Fact]
        public async Task PUT_UserTopicAnswer_Forbidden()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "bot");

            SupportTopicAnswer answer = new();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponsePut($"/api/support-topic/answer", answer.Convert(false), AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopic_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/{DependenciesGuids["SupportTopic"]}", 
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopic_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/{DependenciesGuids["SupportTopic"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopic_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/{DependenciesGuids["SupportTopic"]}");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("admin")]
        [InlineData("bot")]
        public async Task DELETE_UserTopic_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/{DependenciesGuids["SupportTopic"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopicAnswer_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/answer/{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopicAnswer_NotFoundAnswer()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeUserDependency(DependenciesGuids["User"], "user");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/answer/{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopicAnswer_NotFoundUser()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/answer/{DependenciesGuids["AnswerSupport"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_UserTopicAnswer_Unauthorized()
        {
            //Arrange

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/answer/{DependenciesGuids["AnswerUser"]}");

            //Assert

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("admin")]
        [InlineData("owner")]
        [InlineData("bot")]
        public async Task DELETE_UserTopicAnswer_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeUserDependency(DependenciesGuids["User"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/answer/{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
        [Fact]
        public async Task DELETE_SupportTopicAnswer_OK()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DELETE_SupportTopicAnswer_NotFound()
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], "owner");

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }
        [Fact]
        public async Task DELETE_SupportTopicAnswer_Unauthorized()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("bot")]
        public async Task DELETE_SupportTopicAnswer_Forbidden(string role)
        {
            //Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeUserDependency(DependenciesGuids["Support"], role);
            await InitializeTestDependencies();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseDelete($"/api/support-topic/support/answer/" +
                $"{DependenciesGuids["AnswerUser"]}",
                AccessToken);

            //Assert
            await RemoveUserDependency(DependenciesGuids["User"]);
            await RemoveUserDependency(DependenciesGuids["Support"]);
            await RemoveTestDependencies();

            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            UpdateContext();

            SupportTopic topic = new() { 
                Id = DependenciesGuids["SupportTopic"],
                Title = "AAA",
                Content = "BBB",
                Date = DateTime.UtcNow,
                IsClosed = false,
                UserId = DependenciesGuids["User"]
            };

            SupportTopicAnswer answerUser = new()
            {
                Id = DependenciesGuids["AnswerUser"],
                Content = "AAA",
                Date = DateTime.UtcNow,
                PlaintiffId = DependenciesGuids["User"],
                TopicId = topic.Id,
            };

            SupportTopicAnswer answerSupport = new()
            {
                Id = DependenciesGuids["AnswerSupport"],
                Content = "BBB",
                Date = DateTime.UtcNow,
                PlaintiffId = DependenciesGuids["Support"],
                TopicId = topic.Id,
            };

            await Context.SupportTopics.AddAsync(topic);
            await Context.SupportTopicAnswers.AddAsync(answerUser);
            await Context.SupportTopicAnswers.AddAsync(answerSupport);
            await Context.SaveChangesAsync();

            UpdateContext();
        }

        private async Task RemoveTestDependencies()
        {
            UpdateContext();

            List<SupportTopic> topics = await Context.SupportTopics
                .AsNoTracking()
                .ToListAsync();

            foreach (var topic in topics)
                Context.SupportTopics.Remove(topic);

            await Context.SaveChangesAsync();

            UpdateContext();
        }
        #endregion
    }
}
