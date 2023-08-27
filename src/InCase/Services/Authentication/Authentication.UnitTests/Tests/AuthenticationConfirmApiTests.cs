using Authentication.API.Controllers;
using Authentication.BLL.Exceptions;
using Authentication.DAL.Entities;
using Authentication.UnitTests.Common;
using Authentication.UnitTests.Common.Factory;
using Authentication.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Authentication.UnitTests.Tests
{
    public class AuthenticationConfirmApiTests : TestApiBase
    {
        private readonly Controllers controllerClient
            = new Controllers(new AuthenticationConfirmControllerFactory());
        public AuthenticationConfirmApiTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GET_ConfirmAccount_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            var actionResult = await controller.ConfirmAccount(token);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GET_ConfirmAccount_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            user.Email = "xexexee";
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.ConfirmAccount(token));
        }

        [Fact]
        public async Task GET_ConfirmAccount_EmptyOrNotExistedToken_ThrowsNotFoundException()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            UserRole? userRole = await Context.Roles
                .FirstOrDefaultAsync(x => x.Name == "user");

            UserAdditionalInfo? info = new()
            {
                IsConfirmed = true,
                RoleId = userRole!.Id,
                Role = userRole,
                UserId = guid
            };

            User? user = new()
            {
                Id = guid,
                Login = $"UserApiTest",
                Email = $"UserApiTest@mail.ru",
                AdditionalInfo = info,
                PasswordHash = "123",
                PasswordSalt = "321"
            };

            string token = TokenHelper.CreateEmailToken(user);

            // Act
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<NotFoundException>(
                async () => await controller.ConfirmAccount(token));
        }

        [Fact]
        public async Task GET_UpdateEmail_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            var actionResult = await controller.UpdateEmail("newmail@mail.ru", token);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GET_UpdateEmail_ExistedEmail_ThrowsConflictException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            User user2 = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<ConflictException>(
                async () => await controller.UpdateEmail(user2.Email!, token));
        }

        [Fact]
        public async Task GET_UpdateEmail_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            user.Email = "xexexee"; 
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.UpdateEmail("newmail@mail.ru", token));
        }

        [Fact]
        public async Task GET_UpdateLogin_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            var actionResult = await controller.UpdateLogin("login", token);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GET_UpdateLogin_ExistedLogin_ThrowsConflictException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            User user2 = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<ConflictException>(
                async () => await controller.UpdateLogin(user2.Login!, token));
        }

        [Fact]
        public async Task GET_UpdateLogin_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            user.Email = "xexexee";
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.UpdateLogin("newlogin", token));
        }

        [Fact]
        public async Task GET_UpdatePassword_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            var actionResult = await controller.UpdatePassword("1Alogin", token);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GET_UpdatePassword_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            user.Email = "xexexee";
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.UpdatePassword("1Alogin", token));
        }

        [Fact]
        public async Task GET_UpdatePassword_IncorrectPassword_ThrowsBadRequestException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<BadRequestException>(
                async () => await controller.UpdatePassword("login", token));
        }

        [Fact]
        public async Task DELETE_Delete_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            var actionResult = await controller.Delete(token);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task DELETE_Delete_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            user.Email = "xexexee";
            string token = TokenHelper.CreateEmailToken(user);

            // Act 
            AuthenticationConfirmController controller = 
                (AuthenticationConfirmController) controllerClient
                .Create(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.Delete(token));
        }
    }
}
