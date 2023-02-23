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
    public class EmailTokenSendingController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public EmailTokenSendingController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailHelper emailHelper,
            JwtHelper jwtHelper,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _emailHelper = emailHelper;
            _jwtHelper = jwtHelper;
            _validationService = validationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendConfirmEmail(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != data.UserEmail) return Forbid();

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;
            data.EmailToken = _jwtHelper.GenerateEmailToken(user);

            if (userInfo.IsConfirmedAccount is false)
            {
                userInfo.IsConfirmedAccount = true;

                await _emailHelper.SendSuccessVerifedAccount(data, user.UserLogin!);

                await context.SaveChangesAsync();

                return Ok(new { Data = "You can join account", Success = true });
            }
            else
                await _emailHelper.SendLoginAttempt(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPost("email/confirm/{email}")]
        public async Task<IActionResult> SendConfirmNewEmail(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });

            bool isValidToken = _validationService.IsValidEmailToken(in data, in user);
            if (isValidToken is false) return Forbid("Invalid email token");

            data.EmailToken = _jwtHelper.GenerateEmailToken(user);

            MapEmailModelForSend(ref data, in user);
            await _emailHelper.SendConfirmNewEmail(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> SendForgotPassword(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != data.UserEmail) return Forbid();

            data.EmailToken = _jwtHelper.GenerateEmailToken(user);

            await _emailHelper.SendChangePassword(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> SendChangeEmail(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!_validationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailHelper.SendChangeEmail(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }
        
        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> SendChangePassword(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!_validationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailHelper.SendChangePassword(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpDelete("{password}")]
        public async Task<IActionResult> SendDeleteAccount(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.UserId);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!_validationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailHelper.SendDeleteAccount(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        private void MapEmailModelForSend(ref DataMailLink data, in User user)
        {
            data.UserEmail = user.UserEmail!;
            data.UserId = user.Id;
            data.EmailToken = _jwtHelper.GenerateEmailToken(user);
        }
    }
}
