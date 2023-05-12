﻿using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Authentication.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly AuthenticationService _authService;
        #endregion
        #region ctor
        public AuthenticationController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            JwtService jwtService,
            EmailService emailService,
            AuthenticationService authService)
        {
            _contextFactory = contextFactory;
            _jwtService = jwtService;
            _emailService = emailService;
            _authService = authService;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserDto userDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            User user = await context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => 
                u.Id == userDto.Id ||
                u.Email == userDto.Email ||
                u.Login == userDto.Login, cancellationToken) ?? 
                throw new NotFoundCodeException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!))
                throw new ForbiddenCodeException("Неверный пароль");

            await AuthenticationService.CheckUserForBan(user.Id, context);

            return user.AdditionalInfo!.IsConfirmed ? 
                await _emailService.SendToEmail(user.Email!, "Подтверждение входа", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Подтвердите вход в аккаунт с устройства {userDto.Platform!}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                }) :
                await _emailService.SendToEmail(user.Email!, "Подтверждение регистрации", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    BodyButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserDto userDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (await context.Users.AnyAsync(u => u.Email == userDto.Email || u.Login == userDto.Login, cancellationToken))
                throw new ConflictCodeException("Пользователь уже существует");

            User user = userDto.Convert();
            AuthenticationService.CreateNewPassword(ref user, userDto.Password!);

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Name == "user", cancellationToken);

            UserAdditionalInfo info = new() {
                RoleId = role!.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
            };
            
            await _emailService.SendToEmail(user.Email!, "Подтверждение регистрации", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    BodyButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                });

            await context.Users.AddAsync(user, cancellationToken);
            await context.UserAdditionalInfos.AddAsync(info, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.SentEmail();
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string refreshToken, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            User user = await _authService.GetUserFromToken(refreshToken, "refresh", context);

            await AuthenticationService.CheckUserForBan(user.Id, context);

            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user);

            return ResponseUtil.Ok(tokenModel);
        }
    }
}