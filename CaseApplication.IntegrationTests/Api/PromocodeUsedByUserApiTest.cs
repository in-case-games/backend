using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
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
    public class PromocodeUsedByUserApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public PromocodeUsedByUserApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetPromocodeUsedByUser_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);
            await InitializeTestDependencies();

            //Act
            List<PromocodesUsedByUser> promocodesUsed = (await _response
                .ResponseGet<List<PromocodesUsedByUser>>(
                "/PromocodesUsedByUser/all", AccessToken))!;
            HttpStatusCode statusCodeGet = await _response.ResponseGetStatusCode(
                $"/PromocodesUsedByUser/{promocodesUsed[0].Id}", AccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await DeleteTestDependencies();
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task GET_GetAllPromocodesUsedByUser_ReturnsOk()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            await InitializeTestUser(guid);
            await InitializeTestDependencies();

            //Act
            List<PromocodesUsedByUser> promocodesUsed = (await _response
                .ResponseGet<List<PromocodesUsedByUser>>(
                "/PromocodesUsedByUser/all", AccessToken))!;
            HttpStatusCode statusCodeGet = promocodesUsed.Count > 0 ? 
                HttpStatusCode.OK : HttpStatusCode.NotFound;

            //Assert
            Assert.Equal(HttpStatusCode.OK, statusCodeGet);

            await DeleteTestDependencies();
            await RemoveTestUser(guid);
        }

        #region Начальные данные
        private async Task InitializeTestDependencies()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();
            PromocodeType? type = await _response.ResponseGet<PromocodeType>("/PromocodeType/balance");
            Promocode promocode = new()
            {
                PromocodeDiscount = 0.1m,
                PromocodeName = "KAKASHKA",
                PromocodeUsesCount = 1,
                PromocodeTypeId = type!.Id,
            };
            PromocodeDto promocodeDto = mapper.Map<PromocodeDto>(promocode);
            await _response.ResponsePostStatusCode(
                "/Promocode/admin", promocodeDto, AccessToken);
            await _response.ResponseGetStatusCode(
                $"/Promocode/use/{promocode.PromocodeName}", AccessToken);
        }

        private async Task DeleteTestDependencies()
        {
            Promocode? promocode = await _response.ResponseGet<Promocode>(
                $"/Promocode/KAKASHKA", AccessToken);
            await _response.ResponseDelete(
                $"/Promocode/admin/{promocode!.Id}", AccessToken);
        }
        #endregion
    }
}
