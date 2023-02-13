using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class PromocodeTypeApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public PromocodeTypeApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetByNamePromocodeType_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = new() { 
                PromocodeTypeName = "IZI WIN"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/PromocodeType/admin", type, AccessToken);
            type = await _response.ResponseGet<PromocodeType>(
                $"/PromocodeType/{type.PromocodeTypeName}");
            HttpStatusCode statusCodeGet = type is null ?
                HttpStatusCode.NotFound : HttpStatusCode.OK;
            await _response.ResponseDelete(
                $"/PromocodeType/admin/{type!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetAllPromocodeType_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            //Act
            List<PromocodeType> types = (await _response.ResponseGet<List<PromocodeType>>(
                $"/PromocodeType/all"))!;
            HttpStatusCode statusCodeGet = types.Count == 0 ?
                HttpStatusCode.NotFound : HttpStatusCode.OK;

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task POST_CreatePromocodeType_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = new()
            {
                PromocodeTypeName = "IZI WIN"
            };

            //Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/PromocodeType/admin", type, AccessToken);
            type = await _response.ResponseGet<PromocodeType>(
                $"/PromocodeType/{type.PromocodeTypeName}");
            await _response.ResponseDelete(
                $"/PromocodeType/admin/{type!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdatePromocodeType_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = new()
            {
                PromocodeTypeName = "IZI WIN"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/PromocodeType/admin", type, AccessToken);
            type = await _response.ResponseGet<PromocodeType>(
                $"/PromocodeType/{type.PromocodeTypeName}");
            type!.PromocodeTypeName = "IZI LOSS";
            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                "/PromocodeType/admin", type, AccessToken);
            await _response.ResponseDelete(
                $"/PromocodeType/admin/{type!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task Delete_DeletePromocodeType_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = new()
            {
                PromocodeTypeName = "IZI WIN"
            };

            //Act
            await _response.ResponsePostStatusCode(
                "/PromocodeType/admin", type, AccessToken);
            type = await _response.ResponseGet<PromocodeType>(
                $"/PromocodeType/{type.PromocodeTypeName}");
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/PromocodeType/admin/{type!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
