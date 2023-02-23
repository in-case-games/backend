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
        public async Task<IActionResult> SendConfirm(DataMailLink emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid();

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;
            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);

            if (userInfo.IsConfirmedAccount is false)
            {
                userInfo.IsConfirmedAccount = true;

                await _emailHelper.SendConfirmationAccountToEmail(
                    new DataMailLink()
                    {
                        UserEmail = user.UserEmail!
                    }
                    , user.UserLogin!);

                await context.SaveChangesAsync();

                return Ok(new { Data = "You can join account", Success = true });
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

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> SendConfirmForgotPassword(DataMailLink emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid();

            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);

            await _emailHelper.SendChangePasswordToEmail(emailModel, user.UserLogin!);

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        //TODO SendConfirmUpdateEmail => SendConfirmNewEmail => UpdateEmail
        //TODO SendConfirmNewEmail it's confirm email another

        [Authorize]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> SendConfirmUpdateEmail(DataMailLink emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new { Error = ex.Message, Success = false });
            }

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }
        
        [Authorize]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> SendConfirmUpdatePassword(DataMailLink emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new { Error = ex.Message, Success = false });
            }

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        [Authorize]
        [HttpDelete("{password}")]
        public async Task<IActionResult> SendConfirmDeleteAccount(DataMailLink emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new { Error = ex.Message, Success = false });
            }

            return Accepted(new { Message = "Message was sended on your email", Success = true });
        }

        private async Task Send(DataMailLink emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
                throw new NullReferenceException("The user was not found");
            if (!_validationService.IsValidUserPassword(in user, password))
                throw new ArgumentException("Invalid data");

            MapEmailModelForSend(ref emailModel, user);
            await _emailHelper.SendDeleteAccountToEmail(emailModel, user.UserLogin!);
        }

        private void MapEmailModelForSend(ref DataMailLink emailModel, User user)
        {
            emailModel.UserEmail = user.UserEmail!;
            emailModel.UserId = user.Id;
            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);
        }
    }
}
