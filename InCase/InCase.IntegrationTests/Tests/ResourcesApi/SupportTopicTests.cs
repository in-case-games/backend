using InCase.Domain.Entities.Resources;
using InCase.IntegrationTests.Services;
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
            _responseService = new(webApplicationFactory.CreateClient());
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
