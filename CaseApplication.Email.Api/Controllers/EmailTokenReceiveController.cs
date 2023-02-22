using CaseApplication.Domain.Entities.Auth;
using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Email.Api.Controllers
{
    [Route("email/api/[controller]")]
    [ApiController]
    public class EmailTokenReceiveController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly ValidationService _validationService;
        private readonly EncryptorHelper _encryptorHelper;

        public EmailTokenReceiveController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            EmailHelper emailHelper,
            ValidationService validationService,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper)
        {
            _contextFactory = contextFactory;
            _emailHelper = emailHelper;
            _validationService = validationService;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
        }

        //TODO
        [AllowAnonymous]
        [HttpGet("confirm/{userId}&{token}")]
        public async Task<IActionResult> ConfirmAccount(
            Guid userId,
            string token,
            string ip = "",
            string platform = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //TODO OneTimeToken
            DataMailLink emailModel = new()
            {
                UserId = userId,
                EmailToken = token,
                UserIp = ip,
                UserPlatforms = platform
            };

            User? user = await context.User
                .Include(x => x.UserTokens)
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;

            if (userInfo.IsConfirmedAccount is false)
            {
                userInfo.IsConfirmedAccount = true;

                await _emailHelper.SendConfirmationAccountToEmail(
                    new DataMailLink()
                    {
                        UserEmail = user.UserEmail!
                    }
                    , user.UserLogin!);
            }
            else
            {
                await _emailHelper.SendAccountLoginAttempt(
                    new DataMailLink()
                    {
                        UserEmail = user.UserEmail!
                    }
                    , user.UserLogin!);
            }

            //Generate tokens
            DataSendTokens tokenModel = _jwtHelper.GenerateTokenPair(in user);

            UserToken newUserToken = new()
            {
                Id = new Guid(),
                UserId = user.Id,
                UserIpAddress = emailModel.UserIp,
                UserPlatfrom = emailModel.UserPlatforms,
                EmailToken = emailModel.EmailToken,
            };

            MapUserTokenForUpdate(ref newUserToken, tokenModel);

            await context.UserToken.AddAsync(newUserToken);
            await context.SaveChangesAsync();

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(DataMailLink emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistEmail = await context.User
                .AnyAsync(x => x.UserEmail == emailModel.UserEmail);

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (isExistEmail) return Forbid("Email is already busy");
            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);
            if (isValidToken is false) return Forbid("Invalid email token");

            user.UserEmail = emailModel.UserEmail;
            user.UserAdditionalInfo!.IsConfirmedAccount = false;

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                emailModel.UserEmail,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы изменили email аккаунта"
                });
            return Ok(new { Success = true, Message = "Email was changed" });
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePasswordConfirmation(DataMailLink emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context.User
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);
            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

            //Gen hash and salt
            byte[] salt = EncryptorHelper.GenerationSaltTo64Bytes();
            string hash = EncryptorHelper.EncryptorPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы сменили пароль"
                });

            return Ok(new { Success = true, Message = "Password was changed" });
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmation(DataMailLink emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Ваш аккаунт будет удален через 30 дней"
                });

            //TODO No delete give the user 30 days

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Request for delete account was confirmated." });
        }

        private static void MapUserTokenForUpdate(ref UserToken userToken, DataSendTokens tokenModel)
        {
            userToken.RefreshToken = tokenModel.RefreshToken;
            userToken.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
            userToken.RefreshTokenCreationTime = DateTime.UtcNow;
        }
    }
}
