﻿using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
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

            //TODO CUT
            if(userInfo.DeletionDate != null)
            {
                //TODO Send cancel deleted account
                userInfo.DeletionDate = null;

                await context.SaveChangesAsync();
            }

            if (userInfo.IsConfirmed)
            {
                await _emailService.SendLoginAttempt(
                    new DataMailLink()
                    {
                        UserEmail = user.Email!,
                        UserLogin = user.Login!
                    });

                DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user);

                return ResponseUtil.Ok(tokenModel);
            }

            userInfo.IsConfirmed = true;

            await context.SaveChangesAsync();

            return await _emailService.SendSuccessVerifedAccount(
                new DataMailLink()
                {
                    UserEmail = user.Email!,
                    UserLogin = user.Login!
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

            return await _emailService.SendNotifyToEmail(
                email,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы изменили email аккаунта"
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

            return await _emailService.SendNotifyToEmail(
                user.Email!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы сменили пароль"
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

            return await _emailService.SendNotifyToEmail(
                user.Email!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Ваш аккаунт будет удален через 30 дней"
                });
        }
    }
}