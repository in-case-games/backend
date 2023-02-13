using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class PromocodeApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public PromocodeApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_UsePromocode_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);

            //Act
            await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            HttpStatusCode statusCodeUse = await _response.ResponseGetStatusCode(
                $"/Promocode/use/{promocode.PromocodeName}", AccessToken);
            promocode = (await _response.ResponseGet<Promocode>(
                $"/Promocode/{promocode.PromocodeName}", AccessToken))!;
            await _response.ResponseDelete(
                $"/Promocode/admin/{promocode.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUse);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetByNamePromocode_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode? promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);

            //Act
            await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            promocode = await _response.ResponseGet<Promocode>(
                $"/Promocode/{promocode.PromocodeName}", AccessToken);
            HttpStatusCode statusCodeGet = promocode is null ? HttpStatusCode.NotFound : HttpStatusCode.OK;
            await _response.ResponseDelete(
                $"/Promocode/admin/{promocode!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task POST_CreatePromocode_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode? promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);

            //Act
            HttpStatusCode statusCodeCreate = await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            promocode = await _response.ResponseGet<Promocode>(
                $"/Promocode/{promocode.PromocodeName}", AccessToken);
            await _response.ResponseDelete(
                $"/Promocode/admin/{promocode!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeCreate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdatePromocode_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode? promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);

            //Act
            await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            promocode = await _response.ResponseGet<Promocode>(
                $"/Promocode/{promocode.PromocodeName}", AccessToken);
            promocodeDto.Id = promocode!.Id;
            promocodeDto.PromocodeDiscount = 0.2m;
            HttpStatusCode statusCodeUpdate = await _response.ResponsePut(
                "/Promocode/admin", promocodeDto, AccessToken);
            await _response.ResponseDelete(
                $"/Promocode/admin/{promocode!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeUpdate);

            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task DELETE_DeletePromocode_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);

            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode? promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);

            //Act
            await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            promocode = await _response.ResponseGet<Promocode>(
                $"/Promocode/{promocode.PromocodeName}", AccessToken);
            HttpStatusCode statusCodeDelete = await _response.ResponseDelete(
                $"/Promocode/admin/{promocode!.Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);

            await RemoveTestUser(guid);
        }
    }
}
