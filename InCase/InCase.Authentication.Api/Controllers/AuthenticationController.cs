using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InCase.Authentication.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        #endregion
        #region ctor
        public AuthenticationController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            JwtService jwtService,
            EmailService emailService)
        {
            _contextFactory = contextFactory;
            _jwtService = jwtService;
            _emailService = emailService;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                x.Id == userDto.Id ||
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (user is null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!))
                return ResponseUtil.Conflict("Invalid data");

            List<UserRestriction> bans = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == user.Id && w.Type!.Name == "ban")
                .ToListAsync();

            if (bans.Count > 0)
                return ResponseUtil.Conflict(bans);
            if(!user.AdditionalInfo!.IsConfirmed)
                return await _emailService.SendToEmail(user.Email!,
                    "Подтверждение регистрации",
                    new()
                    {
                        BodyTitle = $"Дорогой {user.Login!}",
                        BodyDescription = $"Для завершения этапа регистрации, " +
                        $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                        $"Если это были не вы, проигнорируйте это сообщение.",
                        BodyButtonLink = $"/api/email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                    });

            return await _emailService.SendToEmail(user.Email!,
                "Подтверждение входа",
                new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Подтвердите вход в аккаунт с устройства {userDto.Platform!}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExist = await context.Users
                .AsNoTracking()
                .AnyAsync(x =>
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (isExist) 
                return ResponseUtil.Conflict("User already exists!");

            User user = userDto.Convert();

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(userDto.Password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == "user");

            UserAdditionalInfo info = new() {
                RoleId = role!.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
            };

            try
            {
                await _emailService.SendToEmail(user.Email!,
                    "Подтверждение регистрации",
                    new()
                    {
                        BodyTitle = $"Дорогой {user.Login!}",
                        BodyDescription = $"Для завершения этапа регистрации, " +
                        $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                        $"Если это были не вы, проигнорируйте это сообщение.",
                        BodyButtonLink = $"/api/email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                    });
            }
            catch (SmtpCommandException)
            {
                return ResponseUtil.Conflict("MailBox is not existed!");
            }

            await context.Users.AddAsync(user);
            await context.UserAdditionalInfos.AddAsync(info);

            await context.SaveChangesAsync();

            return ResponseUtil.SendEmail();
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(refreshToken); 

            if(principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(i => i.Restrictions)
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user is null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "refresh"))
                return Forbid("Invalid refresh token");

            List<UserRestriction> bans = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == user.Id && w.Type!.Name == "ban")
                .ToListAsync();

            if (bans.Count > 0)
                return ResponseUtil.Conflict(bans);

            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user!);

            return ResponseUtil.Ok(tokenModel);
        }
    }
}
