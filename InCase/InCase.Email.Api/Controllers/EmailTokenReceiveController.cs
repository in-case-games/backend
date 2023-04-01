using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
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
        [HttpPost("account")]
        public async Task<IActionResult> ConfirmAccount(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(data.EmailToken);

            if (principal is null) return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) return NotFound();
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

                return Ok(new
                {
                    Success = true,
                    Data = tokenModel
                });
            }

            userInfo.IsConfirmed = true;

            await _emailService.SendSuccessVerifedAccount(
                new DataMailLink()
                {
                    UserEmail = user.Email!,
                    UserLogin = user.Login!
                });

            await context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Data = "You can join account"
            });
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(data.EmailToken);

            if (principal is null) return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            bool isExistEmail = await context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == data.UserEmail);

            if (isExistEmail)
                return Conflict(new { Success = false, Message = "Access denied mail is already busy" });

            User? user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) return NotFound();
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            user.Email = data.UserEmail;

            await context.SaveChangesAsync();

            await _emailService.SendNotifyToEmail(
                data.UserEmail,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы изменили email аккаунта"
                });

            return Ok(new 
            { 
                Success = true, 
                Message = "Email was changed" 
            });
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePassword(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(data.EmailToken);

            if (principal is null) return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) return NotFound();
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            //Gen hash and salt
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();
            string hash = EncryptorService.GenerationHashSHA512(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            await context.SaveChangesAsync();

            await _emailService.SendNotifyToEmail(
                user.Email!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы сменили пароль"
                });

            return Ok(new 
            { 
                Success = true, 
                Message = "Password was changed" 
            });
        }

        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(data.EmailToken);

            if (principal is null) return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) return NotFound();
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Access denied invalid email token");

            await _emailService.SendNotifyToEmail(
                user.Email!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Ваш аккаунт будет удален через 30 дней"
                });

            user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            await context.SaveChangesAsync();

            return Ok(new 
            { 
                Success = true, 
                Message = "Request for delete account was confirmated." 
            });
        }
    }
}
