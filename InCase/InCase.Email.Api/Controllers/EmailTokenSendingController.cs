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
            {
                return await _emailService.SendToEmail(user.Email!,
                    "Завершение регистрации в системе",
                    new()
                    {
                        BodyTitle = $"Дорогой {user.Login!}",
                        BodyDescription = $"Для завершения этапа регистрации," +
                        $"вам необходимо нажать на кнопку ниже для подтверждения почты." +
                        $"Если это были не вы, проигнорируйте это сообщение.",
                        BodyButtonLink = $"/api/email/confirm/account?token={data.EmailToken}"
                    });
            }

            return await _emailService.SendToEmail(user.Email!,
                "Подтверждение входа в систему",
                new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Подтвердите вход в аккаунт с устройства {data.UserPlatforms}." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/account?token={data.EmailToken}"
                });
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

            return await _emailService.SendToEmail(data.UserEmail,
                "Подтвердите новую почту аккаунта",
                new()
                {
                    BodyTitle = $"Дорогой, {data.UserLogin}",
                    BodyDescription = $"Подтвердите, что это ваш новый email." +
                    $"Отправка с устройства {data.UserPlatforms}." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта," +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/update/password?token={data.EmailToken}"
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
                return ResponseUtil.NotFound("User");
            if (user.Email != data.UserEmail)
                return ResponseUtil.Conflict("E-mail invalid");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendToEmail(data.UserEmail,
                "Забыли пароль?",
                new()
                {
                    BodyTitle = $"Дорогой {data.UserLogin}",
                    BodyDescription = $"Подтвердите, " +
                    $"что это вы хотите поменять пароль с устройства {data.UserPlatforms}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/update/password?token={data.EmailToken}"
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendToEmail(data.UserEmail,
                "Подтвердите смену почты",
                new()
                {
                    HeaderTitle = "Смена",
                    HeaderSubtitle = "Почты",
                    BodyTitle = $"Дорогой {data.UserLogin}",
                    BodyDescription = $"Подтвердите, " +
                    $"что это вы хотите поменять email с устройства {data.UserPlatforms}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств." +
                    $"<br>" +
                    $"С уважением команда InCase</div>",
                    BodyButtonText = "Подтверждаю",
                    BodyButtonLink = $"/api/email/confirm/update/email?token={data.EmailToken}"
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendToEmail(data.UserEmail,
                "Подтвердите смену пароля",
                new()
                {
                    HeaderTitle = "Смена",
                    HeaderSubtitle = "пароля",
                    BodyTitle = $"Дорогой {data.UserLogin}",
                    BodyDescription = $"Подтвердите, " +
                    $"что это вы хотите поменять пароль с устройства {data.UserPlatforms}. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    BodyButtonLink = $"/api/email/confirm/update/password?token={data.EmailToken}"
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
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, password))
                return Forbid("Invalid data");

            MapDataMailLink(ref data, in user);

            return await _emailService.SendToEmail(data.UserEmail,
                "Подтвердите удаление аккаунта",
                new()
                {
                    HeaderTitle = "Удаление",
                    HeaderSubtitle = "аккаунта",
                    BodyTitle = $"Дорогой {data.UserLogin}",
                    BodyDescription = $"Подтвердите, что это вы удаляете аккаунт. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств. " +
                    $"Мы удалим ваш аккаунт при достижении 30 дней с момента нажатия на эту кнопку.",
                    BodyButtonLink = $"/api/email/confirm/delete?token={data.EmailToken}"
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
