using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => 
                u.Id == userDto.Id ||
                u.Email == userDto.Email ||
                u.Login == userDto.Login);

            if (user is null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!))
                return ResponseUtil.Forbidden("Неверный пароль");

            List<UserRestriction> bans = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == user.Id && ur.Type!.Name == "ban")
                .OrderByDescending(ur => ur.ExpirationDate)
                .ToListAsync();

            if (bans.Count > 0)
                return ResponseUtil.Forbidden($"Вход запрещён до {bans[0].ExpirationDate}. " +
                    $"Причина - {bans[0].Description}");

            return user.AdditionalInfo!.IsConfirmed ? 
                await _emailService.SendToEmail(user.Email!, "Подтверждение входа", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Подтвердите вход в аккаунт с устройства {userDto.Platform!}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                }) :
                await _emailService.SendToEmail(user.Email!, "Подтверждение регистрации", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
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
                .AnyAsync(u => u.Email == userDto.Email || u.Login == userDto.Login);

            if (isExist) 
                return ResponseUtil.Conflict("Пользователь уже существует");

            User user = userDto.Convert();

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(userDto.Password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Name == "user");

            UserAdditionalInfo info = new() {
                RoleId = role!.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
            };

            try
            {
                await _emailService.SendToEmail(user.Email!, "Подтверждение регистрации", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    BodyButtonLink = $"/api/email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                }, false);
            }
            catch (SmtpCommandException)
            {
                return ResponseUtil.Forbidden("Почта не существует или некорректна");
            }

            await context.Users.AddAsync(user);
            await context.UserAdditionalInfos.AddAsync(info);

            await context.SaveChangesAsync();

            return ResponseUtil.SentEmail();
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(refreshToken);

            if (principal is null)
                return ResponseUtil.Unauthorized("Не валидный токен обновления");

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(u => u.Restrictions)
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (user is null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidToken(in user, principal, "refresh"))
                return ResponseUtil.Unauthorized("Не валидный токен обновления");

            List<UserRestriction> bans = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == user.Id && ur.Type!.Name == "ban")
                .OrderByDescending(ur => ur.ExpirationDate)
                .ToListAsync();

            if (bans.Count > 0)
                return ResponseUtil.Forbidden($"Вход запрещён до {bans[0].ExpirationDate}. " +
                    $"Причина - {bans[0].Description}");

            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user!);

            return ResponseUtil.Ok(tokenModel);
        }
    }
}
