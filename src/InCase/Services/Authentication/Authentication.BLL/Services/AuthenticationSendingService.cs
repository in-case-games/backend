using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Authentication.BLL.Exceptions;
using Infrastructure.MassTransit.Email;
using Authentication.BLL.MassTransit;

namespace Authentication.BLL.Services
{
    //TODO ButtonLink edit
    public class AuthenticationSendingService : IAuthenticationSendingService
    {
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public AuthenticationSendingService( 
            IJwtService jwtService,
            ApplicationDbContext context,
            BasePublisher publisher)
        {
            _jwtService = jwtService;
            _context = context;
            _publisher = publisher;
        }

        public async Task DeleteAccountAsync(string login, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            string token = _jwtService.CreateEmailToken(user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите удаление аккаунта",
                Header = new()
                {
                    Title = "Удаление",
                    Subtitle = "аккаунта"
                },
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы удаляете аккаунт. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств. " +
                    $"Мы удалим ваш аккаунт при достижении 30 дней с момента нажатия на эту кнопку.",
                    ButtonLink = $"email/confirm/delete?token={token}"
                }
            };

            await _publisher.SendAsync(template);
        }

        public async Task ForgotPasswordAsync(string login)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            string token = _jwtService.CreateEmailToken(user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Забыли пароль?",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, " +
                    $"что это вы хотите поменять пароль.",
                    ButtonLink = $"email/confirm/update/password?token={token}"
                }
            };

            await _publisher.SendAsync(template);
        }

        public async Task UpdateEmailAsync(string login, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            string token = _jwtService.CreateEmailToken(user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену почты",
                Header = new()
                {
                    Title = "Смена",
                    Subtitle = "Почты",
                },
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять email." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/email?token={token}"
                }
            };

            await _publisher.SendAsync(template);
        }

        public async Task UpdateLoginAsync(string login, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            string token = _jwtService.CreateEmailToken(user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену логина",
                Header = new()
                {
                    Title = "Смена",
                    Subtitle = "Логина",
                },
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять логин." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/login?token={token}"
                }
            };

            await _publisher.SendAsync(template);
        }

        public async Task UpdatePasswordAsync(string login, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            string token = _jwtService.CreateEmailToken(user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену пароля",
                Header = new()
                {
                    Title = "Смена",
                    Subtitle = "пароля",
                },
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять пароль." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/password?token={token}"
                }
            };

            await _publisher.SendAsync(template);
        }
    }
}
