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
    public class EmailTokenSendingController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly ValidationService _validationService;

        public EmailTokenSendingController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailService emailService,
            JwtService jwtService,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _emailService = emailService;
            _jwtService = jwtService;
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
            MapEmailModelForSend(ref data, in user);
            data.EmailToken = _jwtService.GenerateEmailToken(user);

            if (userInfo.IsConfirmedAccount is false)
            {
                await _emailService.SendSignUp(data, user.UserLogin!);
                return Ok(new { Data = "You can join account", Success = true });
            }
            
            await _emailService.SendSignIn(data, user.UserLogin!);

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

            data.EmailToken = _jwtService.GenerateEmailToken(user);

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendConfirmNewEmail(data, user.UserLogin!);

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

            data.EmailToken = _jwtService.GenerateEmailToken(user);

            await _emailService.SendChangePassword(data, user.UserLogin!);

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
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendChangeEmail(data, user.UserLogin!);

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
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendChangePassword(data, user.UserLogin!);

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
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendDeleteAccount(data, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        private void MapEmailModelForSend(ref DataMailLink data, in User user)
        {
            data.UserEmail = user.UserEmail!;
            data.UserId = user.Id;
            data.EmailToken = _jwtService.GenerateEmailToken(user);
        }
    }
}
