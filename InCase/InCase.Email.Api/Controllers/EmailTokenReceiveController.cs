﻿using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Email.Api.Controllers
{
    [Route("api/email/confirm")]
    [ApiController]
    public class EmailTokenReceiveController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        #endregion
        #region ctor
        public EmailTokenReceiveController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailService emailService,
            JwtService jwtService)
        {
            _contextFactory = contextFactory;
            _emailService = emailService;
            _jwtService = jwtService;
        }
        #endregion

        [AllowAnonymous]
        [HttpGet("account")]
        public async Task<IActionResult> ConfirmAccount(string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("User");
            if(!ValidationService.IsValidToken(in user, principal, "email")) 
                return Forbid("Access denied invalid email token");

            UserAdditionalInfo userInfo = user.AdditionalInfo!;

            if(userInfo.DeletionDate != null)
            {
                if(userInfo.IsConfirmed)
                    await _emailService.SendToEmail(user.Email!,
                        "Отмена удаления аккаунта",
                        new()
                        {
                            BodyTitle = $"Дорогой {user.Login!}",
                            BodyDescription = $"Ваш аккаунт больше не в списках на удаление." +
                            $"Спасибо, что остаётесь с нами!"
                        });

                userInfo.DeletionDate = null;

                await context.SaveChangesAsync();
            }

            if (userInfo.IsConfirmed)
            {
                await _emailService.SendToEmail(user.Email!,
                    "Успешный вход в аккаунт",
                    new()
                    {
                        BodyTitle = $"Добро пожаловать {user.Login!}",
                        BodyDescription = $"В ваш аккаунт вошли." +
                        $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                        $"вас автоматически отключит со всех устройств."
                    });

                DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user);

                return ResponseUtil.Ok(tokenModel);
            }

            userInfo.IsConfirmed = true;

            await context.SaveChangesAsync();

            return await _emailService.SendToEmail(user.Email!,
                "Добро пожаловать в InCase!",
                new()
                {
                    HeaderTitle = "Конец этапа",
                    HeaderSubtitle = "регистрации",
                    BodyTitle = $"Добро пожаловать {user.Login!}",
                    BodyDescription = $"Мы рады, что вы новый участник нашего проекта. " +
                    $"Надеемся, что вам понравится наша реализация открытия кейсов. " +
                    $"Подарит множество эмоций и новых предметов."
                });
        }

        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            bool isExistEmail = await context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == email);

            if (isExistEmail) 
                return ResponseUtil.Conflict("E-mail is already busy");

            User? user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            user.Email = email;

            await context.SaveChangesAsync();

            return await _emailService.SendToEmail(email,
                "Ваш аккаунт сменил почту",
                new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Вы изменили email своего аккаунта." +
                    $"Если это были не вы обратитесь в тех поддержку.",
                });
        }

        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            //Gen hash and salt
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();
            string hash = EncryptorService.GenerationHashSHA512(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            await context.SaveChangesAsync();

            return await _emailService.SendToEmail(user.Email!,
                "Ваш аккаунт сменил пароль",
                new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Вы изменили пароль своего аккаунта." +
                    $"Если это были не вы смените пароль," +
                    $"если у вас нет доступа обратитесь в тех поддержку.",
                });
        }

        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            await context.SaveChangesAsync();

            return await _emailService.SendToEmail(user.Email!,
                "Ваш аккаунт будет удален",
                new()
                {
                    BodyTitle = $"Дорогой {user.Login!}.",
                    BodyDescription = $"Ваш аккаунт будет удален в течении 30 дней." +
                    $"Если вы передумали в своем решении просто войдите в аккаунт," +
                    $"и произойдет отмена удаления." +
                    $"Если это не пытались удалить аккаунт срочно поменяйте пароль.",
                });
        }
    }
}
