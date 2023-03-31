using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Email.Api.Controllers
{
    [Route("email/api/[controller]")]
    [ApiController]
    public class EmailTokenReceiveController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly ValidationService _validationService;
        private readonly JwtService _jwtService;
        #endregion
        #region ctor
        public EmailTokenReceiveController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailService emailService,
            ValidationService validationService,
            JwtService jwtService)
        {
            _contextFactory = contextFactory;
            _emailService = emailService;
            _validationService = validationService;
            _jwtService = jwtService;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccount(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null) return NotFound();

            string? secret = user.PasswordHash + user.Email;

            if(!_validationService.IsValidToken(data.EmailToken, secret)) 
                return Forbid("Access denied invalid email token");

            UserAdditionalInfo userInfo = user.AdditionalInfo!;

            if (userInfo.IsConfirmed is false)
            {
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

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistEmail = await context.Users
                .AnyAsync(x => x.Email == data.UserEmail);

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (isExistEmail)
                return Conflict(new { Success = false, Message = "Access denied mail is already busy" });
            if (user == null) 
                return NotFound(new { Success = false, Message = "User not found the update is not available" });

            string secret = user.PasswordHash + user.Email;

            if (!_validationService.IsValidToken(data.EmailToken, secret))
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
        public async Task<IActionResult> UpdatePasswordConfirmation(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null) 
                return NotFound(new { Success = false, Message = "User not found the update is not available" });

            string secret = user.PasswordHash + user.Email;

            if (!_validationService.IsValidToken(data.EmailToken, secret))
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
        public async Task<IActionResult> DeleteConfirmation(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null)
                return NotFound(new { Success = false, Message = "User not found the update is not available" });

            string secret = user.PasswordHash + user.Email;

            if (!_validationService.IsValidToken(data.EmailToken, secret)) 
                return Forbid("Access denied invalid email token");

            await _emailService.SendNotifyToEmail(
                user.Email!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Ваш аккаунт будет удален через 30 дней"
                });

            //TODO No delete give the user 30 days

            return Ok(new 
            { 
                Success = true, 
                Message = "Request for delete account was confirmated." 
            });
        }
    }
}
