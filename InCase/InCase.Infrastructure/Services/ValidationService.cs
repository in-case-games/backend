using InCase.Domain.Entities.Resources;
using System.Security.Claims;

namespace InCase.Infrastructure.Services
{
    public class ValidationService
    {
        private readonly JwtService _jwtService;

        public ValidationService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public static bool IsValidUserPassword(in User user, string password)
        {
            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public bool IsValidToken(string token, string secret)
        {
            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(token, secret);

            return (principal is not null);
        }
    }
}
