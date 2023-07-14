using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit.Models;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Authentication.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            ApplicationDbContext context, 
            IJwtService jwtService,
            IBus bus,
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _bus = bus;
            _configuration = configuration;
        }

        public async Task SignInAsync(UserRequest request)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                u.Id == request.Id || u.Email == request.Email || u.Login == request.Login) ??
                throw new NotFoundException("Пользователь не найден");

            if(!ValidationService.IsValidUserPassword(in user, request.Password))
                throw new ForbiddenException("Неверный пароль");

            await CheckUserForBanAsync(user.Id);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
            };

            if (user.AdditionalInfo!.IsConfirmed)
            {
                template.Subject = "Подтверждение входа";
                template.Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Подтвердите вход в аккаунт. " +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств.",
                    ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                };
            }
            else
            {
                template.Subject = "Подтверждение регистрации";
                template.Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                };
            }

            Uri uri = new(_configuration["MassTransit:Uri"] + "/email");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(template);
        }

        public async Task SignUpAsync(UserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email || u.Login == request.Login))
                throw new ConflictException("Пользователь уже существует");

            User user = request.ToEntity(IsNewGuid: true);
            CreateNewPassword(ref user, request.Password);

            UserRole role = await _context.Roles
                .AsNoTracking()
                .FirstAsync(ur => ur.Name == "user");

            UserAdditionalInfo info = new()
            {
                RoleId = role.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
                IsConfirmed = false,
            };

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Подтверждение регистрации",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Для завершения этапа регистрации, " +
                    $"вам необходимо нажать на кнопку ниже для подтверждения почты. " +
                    $"Если это были не вы, проигнорируйте это сообщение.",
                    ButtonLink = $"email/confirm/account?token={_jwtService.CreateEmailToken(user)}"
                }
            };

            UserTemplate userTemplate = new()
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
                IsDeleted = false
            };

            await _context.Users.AddAsync(user);
            await _context.AdditionalInfos.AddAsync(info);

            await _context.SaveChangesAsync();

            Uri uri = new(_configuration["MassTransit:Uri"] + "/user");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(user);

            uri = new(_configuration["MassTransit:Uri"] + "/email");
            endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(template);
        }

        public async Task<TokensResponse> RefreshTokensAsync(string token)
        {
            User user = await GetUserFromTokenAsync(token, "refresh");

            await CheckUserForBanAsync(user.Id);

            return _jwtService.CreateTokenPair(in user);
        }

        public async Task<User> GetUserFromTokenAsync(string token, string type)
        {
            ClaimsPrincipal principal = _jwtService.GetClaimsToken(token);

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id)) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidToken(in user, principal, type))
                throw new UnauthorizedException($"Не валидный {type} токен");

            return user;
        }

        public async Task CheckUserForBanAsync(Guid id)
        {
            UserRestriction? ban = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.UserId == id);

            if (ban is not null)
            {
                if(ban.ExpirationDate > DateTime.UtcNow)
                    throw new ForbiddenException($"Вход запрещён до {ban.ExpirationDate}.");

                _context.Restrictions.Remove(ban);
                await _context.SaveChangesAsync();
            }
        }

        public static void CreateNewPassword(ref User user, string? password)
        {
            if (password is null) throw new BadRequestException("Пароль некорректный");

            //TODO Check correct password

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(password, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);
        }
    }
}
