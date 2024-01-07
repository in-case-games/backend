using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Authentication.BLL.Exceptions;
using Infrastructure.MassTransit.Email;
using Authentication.BLL.MassTransit;

namespace Authentication.BLL.Services
{
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

        public async Task DeleteAccountAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите удаление аккаунта",
                Header = new EmailHeaderTemplate
                {
                    Title = "Удаление",
                    Subtitle = "аккаунта"
                },
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы удаляете аккаунт. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств. " +
                    $"Мы удалим ваш аккаунт при достижении 30 дней с момента нажатия на эту кнопку.",
                    ButtonLink = $"email/confirm/delete?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);
        }

        public async Task ForgotPasswordAsync(DataMailRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Забыли пароль?",
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, " +
                    $"что это вы хотите поменять пароль.",
                    ButtonLink = $"email/confirm/update/password?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);
        }

        public async Task UpdateEmailAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену почты",
                Header = new EmailHeaderTemplate
                {
                    Title = "Смена",
                    Subtitle = "Почты",
                },
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять email." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/email?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);
        }

        public async Task UpdateLoginAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену логина",
                Header = new EmailHeaderTemplate
                {
                    Title = "Смена",
                    Subtitle = "Логина",
                },
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять логин." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/login?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);
        }

        public async Task UpdatePasswordAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтвердите смену пароля",
                Header = new EmailHeaderTemplate
                {
                    Title = "Смена",
                    Subtitle = "пароля",
                },
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это вы хотите поменять пароль." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/password?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);
        }
    }
}
