using Authentication.API.Controllers;
using Authentication.BLL;
using Authentication.BLL.Exceptions;
using Authentication.BLL.Models;
using Authentication.DAL.Entities;
using Authentication.UnitTests.Common;
using Authentication.UnitTests.Helpers;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Authentication.UnitTests.Tests
{
    public class AuthenticationApiTests : TestApiBase
    {
        public AuthenticationApiTests(ITestOutputHelper output) : base(output) { }
        [Theory]
        [InlineData(HttpStatusCode.OK, "1passwordA", true)]
        public async Task POST_SignIn_ShouldReturnOk(HttpStatusCode code,
            string password, bool shouldCreate)
        {
            // Arrange
            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = "none",
                Email = "none",
                Password = "none"
            };

            if (shouldCreate) 
            {
                User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);

                userRequest.Login = user.Login;
                userRequest.Email = user.Email;
                userRequest.Password = password;
            }

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            var actionResult = await controller.SignIn(userRequest);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)code);
        }
        [Fact]
        public async Task POST_SignIn_NotExistedUser_ThrowsNotFoundException()
        {
            // Arrange
            string password = "1passwordA";

            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = "none",
                Email = "none",
                Password = password
            };

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<NotFoundException>(
                async () => await controller.SignIn(userRequest));
        }
        [Fact]
        public async Task POST_SignIn_InvalidPassword_ThrowsForbiddenException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);

            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email,
                Password = password + "123"
            }; 

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<ForbiddenException>(
                async () => await controller.SignIn(userRequest));
        }
        [Fact]
        public async Task POST_SignIn_BannedUser_ThrowsForbiddenException()
        {
            // Arrange
            string password = "1passwordA";
            Guid userGuid = Guid.NewGuid();

            User user = await Context.InitialiseUser(userGuid,
                password: password, isBanned: true);
            DateTime expTime = new DateTime().AddDays(20);

            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email,
                Password = password
            };

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<ForbiddenException>(
                async () => await controller.SignIn(userRequest));
        }
        [Fact]
        public async Task POST_SignUp_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = "none",
                Email = "none@mail.ru",
                Password = password
            };

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            var actionResult = await controller.SignUp(userRequest);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }
        [Fact]
        public async Task POST_SignUp_IncorrectPassword_ThrowsBadRequestException()
        {
            // Arrange
            string password = "password";

            UserRequest userRequest = new UserRequest()
            {
                Id = Guid.NewGuid(),
                Login = "none",
                Email = "none@mail.ru",
                Password = password
            };

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<BadRequestException>(
                async () => await controller.SignUp(userRequest));
        }
        [Fact]
        public async Task POST_RefreshTokens_ShouldReturnOk()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(), password: password);
            TokensResponse tokens = TokenHelper.CreateTokenPair(user);

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            var actionResult = await controller.RefreshTokens(tokens.RefreshToken!);
            Xunit.Assert.Equal((actionResult as OkObjectResult)?.StatusCode,
                (int)HttpStatusCode.OK);
        }
        [Fact]
        public async Task POST_RefreshTokens_TryOnBannedUser_ThrowsForbiddenException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(),
                password: password, isBanned: true);
            TokensResponse tokens = TokenHelper.CreateTokenPair(user);

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<ForbiddenException>(
                async () => await controller.RefreshTokens(tokens.RefreshToken!));
        }
        [Fact]
        public async Task POST_RefreshTokens_EmptyOrNotExistedToken_ThrowsNotFoundException()
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

            TokensResponse tokens = TokenHelper.CreateTokenPair(user);

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<NotFoundException>(
                async () => await controller.RefreshTokens(tokens.RefreshToken!));
        }
        [Fact]
        public async Task POST_RefreshTokens_InvalidToken_ThrowsUnauthorizedException()
        {
            // Arrange
            string password = "1passwordA";

            User user = await Context.InitialiseUser(Guid.NewGuid(),
                password: password, isBanned: true);
            user.Email = "xexexee";
            TokensResponse tokens = TokenHelper.CreateTokenPair(user);

            // Act
            AuthenticationController controller = ControllerFactory
                .CreateAuthenticationController(Context);

            // Assert
            await Xunit.Assert.ThrowsAsync<UnauthorizedException>(
                async () => await controller.RefreshTokens(tokens.RefreshToken!));
        }
    }
}
