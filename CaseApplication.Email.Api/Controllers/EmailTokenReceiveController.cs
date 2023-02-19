using CaseApplication.Domain.Entities;
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
        private readonly ValidationService _validationService;
        private readonly EncryptorHelper _encryptorHelper;

        public EmailTokenReceiveController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            EmailHelper emailHelper,
            ValidationService validationService,
            EncryptorHelper encryptorHelper)
        {
            _contextFactory = contextFactory;
            _emailHelper = emailHelper;
            _validationService = validationService;
            _encryptorHelper = encryptorHelper;
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(EmailModel emailModel)
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
                new EmailPatternModel()
                {
                    Body = $"Вы изменили email аккаунта"
                });
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePasswordConfirmation(EmailModel emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context.User
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);
            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вы сменили пароль"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmation(EmailModel emailModel)
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
                new EmailPatternModel()
                {
                    Body = $"Ваш аккаунт будет удален через 30 дней"
                });

            //TODO No delete give the user 30 days

            context.UserToken.RemoveRange(user.UserTokens!);

            await context.SaveChangesAsync();

            return Ok();
        }

    }
}
