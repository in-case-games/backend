using CaseApplication.Domain.Entities;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly EmailHelper _emailHelper;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EncryptorHelper encryptorHelper,
            EmailHelper emailHelper,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _encryptorHelper = encryptorHelper;
            _emailHelper = emailHelper;
            _validationService = validationService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";

            return Ok(user);
        }

        [Authorize]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserLogin == login);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<User> users = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                users[i].UserEmail = "access denied";
                users[i].PasswordHash = "access denied";
                users[i].PasswordSalt = "access denied";
            }

            return Ok(users);
        }

        [Authorize]
        [HttpPut("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistLogin = await context.User.AnyAsync(x => x.UserLogin == login);

            User? user = await context.User
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (isExistLogin) return BadRequest();
            if (user == null) return NotFound();

            user.UserLogin = login;

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!, 
                "Администрация сайта" , 
                new EmailPatternModel()
                {
                    Body = $"Имя вашего акканута измененно на: {login}"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(EmailModel emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistEmail = await context.User.AnyAsync(x => x.UserEmail == emailModel.UserEmail);
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

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null) return NotFound();

            context.User.Remove(user);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
