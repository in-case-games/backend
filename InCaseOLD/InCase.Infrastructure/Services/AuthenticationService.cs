using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Infrastructure.Services
{
    public class AuthenticationService
    {
        private readonly JwtService _jwtService;

        public AuthenticationService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        ///<summary>
        /// Reads and validates a JWT and get user
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <param name="type">JWT token type is refresh or email</param>
        /// <exception cref="UnauthorizedCodeException"><paramref name="token"/>Is incorrect or invalid</exception>
        /// <exception cref="NotFoundCodeException">Not found user by token id</exception>
        /// <returns><see cref="User"/> entity</returns>
        public async Task<User> GetUserFromToken(string token, string type, ApplicationDbContext context)
        {
            ClaimsPrincipal principal = _jwtService.GetClaimsToken(token);

            string id = principal.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id)) ??
                throw new NotFoundCodeException("Пользователь не найден");

            if (!ValidationService.IsValidToken(in user, principal, type))
                throw new UnauthorizedCodeException($"Не валидный {type} токен");

            return user;
        }

        ///<summary>
        /// Check user for ban or forbidden code exception
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="context">Context ef core</param>
        /// <exception cref="ForbiddenCodeException"><paramref name="id"/>User for bans</exception>
        public static async Task CheckUserForBan(Guid id, ApplicationDbContext context)
        {
            List<UserRestriction> bans = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == id && ur.Type!.Name == "ban")
                .OrderByDescending(ur => ur.ExpirationDate)
                .ToListAsync();

            if (bans.Count > 0)
                throw new ForbiddenCodeException($"Вход запрещён до {bans[0].ExpirationDate}. " +
                    $"Причина - {bans[0].Description}");
        }

        public static void CreateNewPassword(ref User user, string password)
        {
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(password, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);
        }
    }
}
