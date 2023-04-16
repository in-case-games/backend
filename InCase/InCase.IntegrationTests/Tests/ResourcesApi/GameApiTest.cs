﻿using InCase.IntegrationTests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class GameApiTest : BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public GameApiTest(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }

        [Fact]
        public async Task GET_GetGames_ReturnsOK()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode("/api/game");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GET_GetGame_ReturnsNotFound()
        {
            //Arrange
            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/game/{Guid.NewGuid()}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GET_GetGame_ReturnsOk()
        {
            //Arrange
            AnswerGame? answerGame = await _responseService
                .ResponseGet<AnswerGame>($"/api/game");

            Guid gameId = answerGame?.Data?[0].Id ?? Guid.NewGuid();

            //Act
            HttpStatusCode statusCode = await _responseService
                .ResponseGetStatusCode($"/api/game/{gameId}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        private class AnswerGame
        {
            public bool Success { get; set; }
            public List<Domain.Entities.Resources.Game>? Data { get; set; }
        }
    }
}
