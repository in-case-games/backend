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

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null) return NotFound();
            if (user.Email != data.UserEmail) return Forbid();

            UserAdditionalInfo userInfo = user.AdditionalInfo!;
            MapEmailModelForSend(ref data, in user);

            if (userInfo.IsConfirmed is false)
            {
                await _emailService.SendSignUp(data);
                return Ok(new { Data = "You can join account", Success = true });
            }

            await _emailService.SendSignIn(data);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPost("email/confirm/{email}")]
        public async Task<IActionResult> SendConfirmNewEmail(DataMailLink data, string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null) return NotFound(new 
            { 
                Message = "The user was not found", Success = false 
            });
            string secret = user.PasswordHash + user.Email;

            if (_validationService.IsValidToken(data.EmailToken, secret)) 
            {
                MapEmailModelForSend(ref data, in user);
                data.UserEmail = email;
                await _emailService.SendConfirmNewEmail(data);

                return Accepted(new { Message = "Message was sended on your email", Success = true });
            }

            return Forbid("Invalid email token");
        }

        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> SendForgotPassword(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null) return NotFound();
            if (user.Email != data.UserEmail) return Forbid();

            data.EmailToken = _jwtService.CreateEmailToken(user);

            await _emailService.SendChangePassword(data);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> SendChangeEmail(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendChangeEmail(data);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> SendChangePassword(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendChangePassword(data);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpDelete("{password}")]
        public async Task<IActionResult> SendDeleteAccount(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserName);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapEmailModelForSend(ref data, in user);
            await _emailService.SendDeleteAccount(data);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        private void MapEmailModelForSend(ref DataMailLink data, in User user)
        {
            data.UserEmail = user.Email!;
            data.UserName = user.Login!;
            data.EmailToken = _jwtService.CreateEmailToken(user);
        }
    }
}
