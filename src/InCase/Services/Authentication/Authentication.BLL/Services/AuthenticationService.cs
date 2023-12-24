using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.Email;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Authentication.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public AuthenticationService(
            IJwtService jwtService,
            ApplicationDbContext context, 
            BasePublisher publisher)
        {
            _context = context;
            _jwtService = jwtService;
            _publisher = publisher;
        }

        public async Task SignInAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                u.Id == request.Id || u.Email == request.Email || u.Login == request.Login, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if(!ValidationService.IsValidUserPassword(in user, request.Password))
                throw new ForbiddenException("Неверный пароль");

            await CheckUserForBanAsync(user.Id, cancellationToken);

            await _publisher.SendAsync(new EmailTemplate
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = user.AdditionalInfo!.IsConfirmed ? "Подтверждение входа" : "Подтверждение регистрации",
                Body = user.AdditionalInfo!.IsConfirmed ? 
                    new EmailBodyTemplate
                    {
                        Title = $"Дорогой {user.Login!}",
                        Description = $"Подтвердите вход в аккаунт. " +
                        $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                        $"вас автоматически отключит со всех устройств.",
                        ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                    } : 
                    new EmailBodyTemplate
                    {
                        Title = $"Дорогой {user.Login!}",
                        Description = $"Для завершения этапа регистрации, " +
                        $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                        $"Если это были не вы, проигнорируйте это сообщение.",
                        ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                    }
            }, cancellationToken);
        }

        public async Task SignUpAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email || u.Login == request.Login, cancellationToken))
                throw new ConflictException("Пользователь уже существует");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Login = request.Login
            };

            CreateNewPassword(ref user, request.Password);

            var role = await _context.Roles
                .AsNoTracking()
                .FirstAsync(ur => ur.Name == "user", cancellationToken);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.AdditionalInfos.AddAsync(new UserAdditionalInfo
            {
                RoleId = role.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(1),
                IsConfirmed = false,
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _publisher.SendAsync(user.ToTemplate(false), cancellationToken);
            await _publisher.SendAsync(new EmailTemplate()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтверждение регистрации",
                Body = new EmailBodyTemplate
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                }
            }, cancellationToken);

            FileService.CreateFolder($"users/{user.Id}/");
        }

        public async Task<TokensResponse> RefreshTokensAsync(string token, CancellationToken cancellationToken = default)
        {
            var user = await GetUserFromTokenAsync(token, "refresh", cancellationToken);

            var info = await _context.AdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == user.Id, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (info.DeletionDate is not null)
                throw new ForbiddenException($"Аккаунт в очереди на удаление, отмените входом в аккаунт");

            await CheckUserForBanAsync(user.Id, cancellationToken);

            return _jwtService.CreateTokenPair(in user);
        }

        public async Task<User> GetUserFromTokenAsync(string token, string type, CancellationToken cancellationToken = default)
        {
            var claims = _jwtService.GetClaimsToken(token);

            var id = claims.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id), cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidToken(in user, claims, type))
                throw new UnauthorizedException($"Не валидный {type} токен");

            return user;
        }

        public static void CreateNewPassword(ref User user, string? password)
        {
            ValidationService.CheckCorrectPassword(password);

            var salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSha512(password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);
        }

        private async Task CheckUserForBanAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var ban = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == id, cancellationToken);

            if (ban is not null)
            {
                if (ban.ExpirationDate > DateTime.UtcNow)
                    throw new ForbiddenException($"Вход запрещён до {ban.ExpirationDate}.");

                _context.Restrictions.Remove(ban);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
