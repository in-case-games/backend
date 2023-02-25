using CaseApplication.Domain.Entities.Auth;
using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Email.Api.Controllers
{
    [Route("email/api/[controller]")]
    [ApiController]
    public class EmailTokenReceiveController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly ValidationService _validationService;

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

        //TODO
        [AllowAnonymous]
        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmAccount(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserTokens)
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in data, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;

            if (userInfo.IsConfirmedAccount is false)
            {
                userInfo.IsConfirmedAccount = true;

                await _emailService.SendSuccessVerifedAccount(
                    new DataMailLink()
                    {
                        UserEmail = user.UserEmail!
                    }, 
                    user.UserLogin!);

                await context.SaveChangesAsync();

                return Ok(new { Data = "You can join account", Success = true });
            }
            else
            {
                await _emailService.SendLoginAttempt(
                    new DataMailLink()
                    {
                        UserEmail = user.UserEmail!
                    }
                    , user.UserLogin!);
            }

            //Generate tokens
            DataSendTokens tokenModel = _jwtService.GenerateTokenPair(in user);

            UserToken newUserToken = new()
            {
                Id = new Guid(),
                UserId = user.Id,
                UserIpAddress = data.UserIp,
                UserPlatfrom = data.UserPlatforms,
                EmailToken = data.EmailToken,
            };

            MapUserTokenForUpdate(ref newUserToken, tokenModel);

            await context.UserToken.AddAsync(newUserToken);
            await context.SaveChangesAsync();

            return Ok(new { Data = tokenModel, Success = true });
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistEmail = await context.User
                .AnyAsync(x => x.UserEmail == data.UserEmail);

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (isExistEmail) return Forbid("Email is already busy");
            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in data, in user);
            if (isValidToken is false) return Forbid("Invalid email token");

            user.UserEmail = data.UserEmail;

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            await _emailService.SendNotifyToEmail(
                data.UserEmail,
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
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();
            string hash = EncryptorService.GenerationHashSHA512(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            await _emailService.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Вы сменили пароль"
                });

            return Ok(new { Success = true, Message = "Password was changed" });
        }

        [AllowAnonymous]
        [HttpDelete("account")]
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

            await _emailService.SendNotifyToEmail(
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
