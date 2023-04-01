using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Email.Api.Controllers
{
    [Route("api/email/send")]
    [ApiController]
    public class EmailTokenSendingController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly ValidationService _validationService;
        #endregion
        #region ctor
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
        #endregion

        [AllowAnonymous]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccount(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null) 
                return NotFound(new { Success = false, Message = "User not found the update is not available" });
            if (user.Email != data.UserEmail)
                return Conflict(new { Success = false, Message = "Access denied mail invalid" });

            MapDataMailLink(ref data, in user);

            if (user.AdditionalInfo!.IsConfirmed is false)
            {
                await _emailService.SendSignUp(data);

                return Ok(new 
                {
                    Success = true,
                    Data = "You can join account" 
                });
            }

            await _emailService.SendSignIn(data);

            return Accepted(new 
            {
                Success = true,
                Message = "Message was sended on your email" 
            });
        }

        [AllowAnonymous]
        [HttpPost("confirm/{email}")]
        public async Task<IActionResult> ConfirmNewEmail(DataMailLink data, string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistEmail = await context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == email);

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (isExistEmail)
                return Conflict(new { Success = false, Message = "Access denied mail is already busy" });
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });

            string secret = user.PasswordHash + user.Email;

            if (_validationService.IsValidToken(in user, data.EmailToken, "email"))
                return Forbid("Access denied invalid email token");

            MapDataMailLink(ref data, in user);
            data.UserEmail = email;

            await _emailService.SendConfirmNewEmail(data);

            return Accepted(new 
            {
                Success = true,
                Message = "Message was sended on your email" 
            });
        }

        [AllowAnonymous]
        [HttpPut("forgot/password")]
        public async Task<IActionResult> ForgotPassword(DataMailLink data)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });
            if (user.Email != data.UserEmail)
                return Conflict(new { Success = false, Message = "Access denied mail invalid" });

            MapDataMailLink(ref data, in user);

            await _emailService.SendChangePassword(data);

            return Accepted(new 
            {
                Success = true,
                Message = "Message was sended on your email"
            });
        }

        [AllowAnonymous]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> UpdateEmail(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            await _emailService.SendChangeEmail(data);

            return Accepted(new 
            { 
                Success = true, 
                Message = "Message was sended on your email" 
            });
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePassword(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            await _emailService.SendChangePassword(data);

            return Accepted(new 
            {
                Success = true,
                Message = "Message was sended on your email"
            });
        }

        [AllowAnonymous]
        [HttpDelete("confirm/{password}")]
        public async Task<IActionResult> DeleteAccount(DataMailLink data, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == data.UserLogin);

            if (user == null)
                return NotFound(new { Message = "The user was not found", Success = false });
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);
            await _emailService.SendDeleteAccount(data);

            return Accepted(new 
            {
                Success = true,
                Message = "Message was sended on your email", 
            });
        }

        private void MapDataMailLink(ref DataMailLink data, in User user)
        {
            data.UserEmail = user.Email!;
            data.UserLogin = user.Login!;
            data.EmailToken = _jwtService.CreateEmailToken(user);
        }
    }
}
