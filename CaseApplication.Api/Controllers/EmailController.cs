using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public EmailController(
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
        [HttpPost("user/confirm")]
        public async Task<IActionResult> SendConfirmEmail(EmailModel emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .Include(x => x.UserTokens)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid("Incorrect email");

            emailModel.UserToken = _jwtHelper.GenerateEmailToken(user, emailModel.UserIp);

            await _emailHelper.SendConfirmAccountToEmail(emailModel);

            return Accepted();
        }

        [Authorize]
        [HttpPut("user/email/{password}")]
        public async Task<IActionResult> SendUpdateEmail(EmailModel emailModel, string password)
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
                return NotFound(ex.Message);
            }

            return Accepted();
        }
        
        [Authorize]
        [HttpPut("user/password/{password}")]
        public async Task<IActionResult> SendUpdatePassword(EmailModel emailModel, string password)
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
                return NotFound(ex.Message);
            }

            return Accepted();
        }

        [Authorize]
        [HttpDelete("user/{password}")]
        public async Task<IActionResult> SendDeleteAccount(EmailModel emailModel, string password)
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
                return NotFound(ex.Message);
            }

            return Accepted();
        }
        private async Task Send(EmailModel emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .Include(x => x.UserTokens)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
                throw new NullReferenceException("The user was not found");
            if (!_validationService.IsValidEmailTokenSend(in user, emailModel.UserIp, password))
                throw new ArgumentException("IncorrectPassword");

            MapEmailModelForSend(ref emailModel, user);
            await _emailHelper.SendDeleteAccountToEmail(emailModel);
        }

        private void MapEmailModelForSend(ref EmailModel emailModel, User user)
        {
            emailModel.UserEmail = user.UserEmail!;
            emailModel.UserId = user.Id;
            emailModel.UserToken = _jwtHelper.GenerateEmailToken(user, emailModel.UserIp);
        }
    }
}
