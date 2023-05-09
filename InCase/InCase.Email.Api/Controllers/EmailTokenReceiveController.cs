using InCase.Domain.Entities.Auth;
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
    [Route("api/email/confirm")]
    [ApiController]
    public class EmailTokenReceiveController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        #endregion
        #region ctor
        public EmailTokenReceiveController(
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
        [HttpGet("account")]
        public async Task<IActionResult> ConfirmAccount(string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null) 
                return ResponseUtil.Unauthorized("Не валидный email токен");

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidToken(in user, principal, "email")) 
                return ResponseUtil.Unauthorized("Не валидный email токен");

            UserAdditionalInfo userInfo = user.AdditionalInfo!;

            if (!userInfo.IsConfirmed)
            {
                userInfo.IsConfirmed = true;
                userInfo.DeletionDate = null;

                await context.SaveChangesAsync();

                return await _emailService.SendToEmail(user.Email!, "Добро пожаловать в InCase!", new()
                {
                    HeaderTitle = "Конец этапа",
                    HeaderSubtitle = "регистрации",
                    BodyTitle = $"Добро пожаловать {user.Login!}",
                    BodyDescription = $"Мы рады, что вы новый участник нашего проекта. " +
                    $"Надеемся, что вам понравится наша реализация открытия кейсов. " +
                    $"Подарит множество эмоций и новых предметов."
                });
            }

            if (userInfo.DeletionDate != null)
            {
                await _emailService.SendToEmail(user.Email!, "Отмена удаления аккаунта", new()
                {
                    BodyTitle = $"Дорогой {user.Login!}",
                    BodyDescription = $"Ваш аккаунт больше не в списках на удаление." +
                    $"Спасибо, что остаётесь с нами!"
                });

                userInfo.DeletionDate = null;

                await context.SaveChangesAsync();
            }

            await _emailService.SendToEmail(user.Email!, "Успешный вход в аккаунт", new()
            {
                BodyTitle = $"Добро пожаловать {user.Login!}",
                BodyDescription = $"В ваш аккаунт вошли." +
                $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                $"вас автоматически отключит со всех устройств."
            });

            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user);

            return ResponseUtil.Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null)
                return ResponseUtil.Unauthorized("Не валидный email токен");

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            bool isExistEmail = await context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);

            if (isExistEmail) 
                return ResponseUtil.Conflict("Email почта занята");

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return ResponseUtil.Unauthorized("Не валидный email токен");

            user.Email = email;

            await context.SaveChangesAsync();
            await _emailService.SendToEmail(email, "Ваш аккаунт сменил почту", new()
            {
                BodyTitle = $"Дорогой {user.Login!}",
                BodyDescription = $"Вы изменили email своего аккаунта." +
                $"Если это были не вы обратитесь в тех поддержку.",
            });

            return ResponseUtil.Ok(user.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null)
                return ResponseUtil.Unauthorized("Не валидный email токен");

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return ResponseUtil.Unauthorized("Не валидный email токен");

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();
            string hash = EncryptorService.GenerationHashSHA512(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            await context.SaveChangesAsync();
            await _emailService.SendToEmail(user.Email!, "Ваш аккаунт сменил пароль", new()
            {
                BodyTitle = $"Дорогой {user.Login!}",
                BodyDescription = $"Вы изменили пароль своего аккаунта." +
                $"Если это были не вы смените пароль," +
                $"если у вас нет доступа обратитесь в тех поддержку.",
            });

            return ResponseUtil.Ok(user.Convert(false));
        }

        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token);

            if (principal is null)
                return ResponseUtil.Unauthorized("Не валидный email токен");

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (user == null) 
                return ResponseUtil.NotFound("Пользователь не найден");
            if (!ValidationService.IsValidToken(in user, principal, "email"))
                return ResponseUtil.Unauthorized("Не валидный email токен");

            user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            await context.SaveChangesAsync();
            await _emailService.SendToEmail(user.Email!, "Ваш аккаунт будет удален", new()
            {
                BodyTitle = $"Дорогой {user.Login!}.",
                BodyDescription = $"Ваш аккаунт будет удален в течении 30 дней." +
                $"Если вы передумали в своем решении просто войдите в аккаунт," +
                $"и произойдет отмена удаления." +
                $"Если это не пытались удалить аккаунт срочно поменяйте пароль.",
            });

            return ResponseUtil.Accepted(user.Convert(false));
        }
    }
}
