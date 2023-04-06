using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        #endregion
        #region ctor
        public EmailTokenSendingController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailService emailService,
            JwtService jwtService)
        {
            _contextFactory = contextFactory;
            _emailService = emailService;
            _jwtService = jwtService;
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
                return ResponseUtil.NotFound("User");
            if (user.Email != data.UserEmail) 
                return ResponseUtil.Conflict("E-mail invalid");

            MapDataMailLink(ref data, in user);

            if (user.AdditionalInfo!.IsConfirmed is false)
                return await _emailService.SendSignUp(data);

            return await _emailService.SendSignIn(data);
        }

        [AllowAnonymous]
        [HttpPost("confirm/{email}")]
        public async Task<IActionResult> ConfirmNewEmail(DataMailLink data, string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(data.EmailToken);

            if (principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            bool isExistEmail = await context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == email);

            if (isExistEmail) 
                return ResponseUtil.Conflict("E-mail is already busy");

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return Forbid("Invalid email token");

            MapDataMailLink(ref data, in user);
            data.UserEmail = email;

            return await _emailService.SendConfirmNewEmail(data);
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
                return ResponseUtil.NotFound("User");
            if (user.Email != data.UserEmail)
                return ResponseUtil.Conflict("E-mail invalid");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendChangePassword(data);
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendChangeEmail(data);
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendChangePassword(data);
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendDeleteAccount(data);
        }

        private void MapDataMailLink(ref DataMailLink data, in User user)
        {
            data.UserEmail = user.Email!;
            data.UserLogin = user.Login!;
            data.EmailToken = _jwtService.CreateEmailToken(user);
        }
    }
}
