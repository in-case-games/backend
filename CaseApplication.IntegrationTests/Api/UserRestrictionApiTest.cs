using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class UserRestrictionApiTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ResponseHelper _response;
        public UserRestrictionApiTest(WebApplicationFactory<Program> application)
        {
            _response = new ResponseHelper(application.CreateClient());
        }
        private async Task<UserRestriction> CreateUserRestriction()
        {
            User user = InitializeUser();
            await _response.ResponsePost<User>("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User?email={user.UserEmail}&hash=123");
            UserRestriction userRestriction = new UserRestriction()
            {
                UserId = currentUser.Id,
                RestrictionName = GenerateString(),
                CreatedDate = DateTime.Now
            };

            await _response.ResponsePost<UserRestriction>("/UserRestriction", userRestriction);
            IEnumerable<UserRestriction> userRestrictions = await _response
                .ResponseGet<List<UserRestriction>>
                ($"/UserRestriction/GetAllRestrictions?userId={currentUser.Id}");

            return userRestrictions!.FirstOrDefault(x => x.UserId == currentUser.Id)!;
        }
        private User InitializeUser()
        {
            User user = new User
            {
                UserName = GenerateString(),
                UserEmail = GenerateString(),
                UserImage = GenerateString(),
                PasswordHash = GenerateString(),
                PasswordSalt = GenerateString(),
            };

            return user;
        }
        private string GenerateString()
        {
            byte[] bytes = new byte[10];
            new Random().NextBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
        private async Task DeleteUser(params Guid[] userIds)
        {
            foreach (Guid userId in userIds)
            {
                await _response.ResponseDelete($"/User?id={userId}");
            }

        }
        [Fact]
        public async Task GetUserRestrictionByIdTest()
        {
            // Arrange
            UserRestriction userRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode($"/UserRestriction?id={userRestriction.Id}");
            await DeleteUser(userRestriction.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetAllUserRestrictionsTest()
        {
            // Arrange
            UserRestriction userRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseGetStatusCode
                ($"/UserRestriction/GetAllRestrictions?userId={userRestriction.UserId}");
            await DeleteUser(userRestriction.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task GetNotExistedUserRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = new UserRestriction();

            // Act
            HttpStatusCode getAllStatusCode = await _response
                .ResponseGetStatusCode
                ($"/UserRestriction/GetAllRestrictions?userId={userRestriction.UserId}");
            HttpStatusCode getOneStatusCode = await _response
                .ResponseGetStatusCode($"/UserRestriction?id={userRestriction.Id}");

            // Assert
            Assert.NotEqual((HttpStatusCode.OK, HttpStatusCode.OK),
                (getOneStatusCode, getAllStatusCode));
        }
        [Fact]
        public async Task PostUserRestrictionTest()
        {
            // Arrange
            User user = InitializeUser();
            await _response.ResponsePost<User>("/User", user);
            User currentUser = await _response
                .ResponseGet<User>($"/User?email={user.UserEmail}&hash=123");
            UserRestriction userRestriction = new UserRestriction()
            {
                UserId = currentUser.Id,
                RestrictionName = GenerateString(),
                CreatedDate = DateTime.Now
            };

            HttpStatusCode statusCode = await _response.ResponsePost<UserRestriction>("/UserRestriction", userRestriction);
            await DeleteUser(currentUser.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PostEmptyUserRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = new UserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePost<UserRestriction>("/UserRestriction", userRestriction);

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutUserRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut<UserRestriction>("/UserRestriction", userRestriction);
            await DeleteUser(userRestriction.UserId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task PutEmptyRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = new UserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponsePut<UserRestriction>("/UserRestriction", userRestriction);

            //
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteUserRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = await CreateUserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/UserRestriction?id={userRestriction.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        [Fact]
        public async Task DeleteNotExistedUserRestrictionTest()
        {
            // Arrange
            UserRestriction userRestriction = new UserRestriction();

            // Act
            HttpStatusCode statusCode = await _response
                .ResponseDelete($"/UserRestriction?id={userRestriction.Id}");

            // Assert
            Assert.NotEqual(HttpStatusCode.OK, statusCode);
        }
    }
}
