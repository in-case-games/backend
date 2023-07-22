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
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public AuthenticationSendingService(
            IAuthenticationService authenticationService, 
            IJwtService jwtService,
            ApplicationDbContext context,
            BasePublisher publisher)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _context = context;
            _publisher = publisher;
        }

        public async Task ConfirmAccountAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || request.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            request.Email = user.Email!;

            MapDataMailRequest(ref request, in user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
            };

            if (user.AdditionalInfo!.IsConfirmed)
            {
                template.Subject = "Подтверждение входа в систему";
                template.Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Подтвердите вход в аккаунт." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonLink = $"email/confirm/account?token={request.Token}"
                };
            }
            else
            {
                template.Subject = "Завершение регистрации в системе";
                template.Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Для завершения этапа регистрации," +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты." +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    ButtonLink = $"email/confirm/account?token={request.Token}"
                };
            }

            await _publisher.SendAsync(template, "/email");
        }

        public async Task ConfirmNewEmailAsync(DataMailRequest request, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User user = await _authenticationService.GetUserFromTokenAsync(request.Token, "email");

            MapDataMailRequest(ref request, in user);
            request.Email = email;

            EmailTemplate template = new()
            {
                Email = email,
                IsRequiredMessage = true,
                Subject = "Подтвердите новую почту аккаунта",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Подтвердите, что это ваш новый email." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта," +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonLink = $"email/confirm/update/password?token={request.Token}"
                }
            };

            await _publisher.SendAsync(template, "/email");
        }

        public async Task DeleteAccountAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            EmailTemplate template = new()
            {
                Email = request.Email,
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
                    ButtonLink = $"email/confirm/delete?token={request.Token}"
                }
            };

            await _publisher.SendAsync(template, "/email");
        }

        public async Task ForgotPasswordAsync(DataMailRequest request)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            MapDataMailRequest(ref request, in user);

            request.Email = user.Email!;

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
                    ButtonLink = $"email/confirm/update/password?token={request.Token}"
                }
            };

            await _publisher.SendAsync(template, "/email");
        }

        public async Task UpdateEmailAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            EmailTemplate template = new()
            {
                Email = request.Email!,
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
                    $"вас автоматически отключит со всех устройств.<br>" +
                    $"С уважением команда InCase</div>",
                    ButtonText = "Подтверждаю",
                    ButtonLink = $"email/confirm/update/email?token={request.Token}"
                }
            };

            await _publisher.SendAsync(template, "/email");
        }

        public async Task UpdatePasswordAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            EmailTemplate template = new()
            {
                Email = request.Email!,
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
                    ButtonLink = $"email/confirm/update/password?token={request.Token}"
                }
            };

            await _publisher.SendAsync(template, "/email");
        }

        private void MapDataMailRequest(ref DataMailRequest request, in User user)
        {
            request.Email = user.Email!;
            request.Login = user.Login!;
            request.Token = _jwtService.CreateEmailToken(user);
        }
    }
}
