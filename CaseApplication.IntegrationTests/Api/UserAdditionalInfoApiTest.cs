using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class UserAdditionalInfoApiTest : IntegrationTestHelper, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;

        public UserAdditionalInfoApiTest(WebApplicationFactory<Program> app)
        {
            _response = new(app.CreateClient());
        }

        [Fact]
        public async Task GET_GetAuthorizeUserInfo_ReturnsOk()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode("/UserAdditionalInfo/", AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }

        [Fact]
        public async Task PUT_UpdateUserByAdmin_ReturnsOk()
        {
            IMapper mapper = MapperConfiguration.CreateMapper();

            // Arrange
            Guid guid = Guid.NewGuid();
            await InitializeTestUser(guid);

            User? user = await _response
                .ResponseGet<User>($"/User/login/{User.UserLogin}", AccessToken);
            
            UserAdditionalInfoDto info = mapper.Map<UserAdditionalInfoDto>(user!.UserAdditionalInfo);
            info.UserRoleId = user!.UserAdditionalInfo!.UserRole!.Id;
            info.UserId = user.Id;

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut("/UserAdditionalInfo/admin", info, AccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
            await RemoveTestUser(guid);
        }
    }
}
