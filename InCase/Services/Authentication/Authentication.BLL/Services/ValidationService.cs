using System.Security.Claims;

namespace Authentication.BLL.Services
{
    public class ValidationService
    {
        public static bool IsValidUserPassword(in User user, string password)
        {
            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public static bool IsValidToken(in User user, ClaimsPrincipal principal, string type)
        {
            string? lifetime = principal?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            DateTimeOffset lifetimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(lifetime ?? "0"));
            DateTime lifetimeDateTime = lifetimeOffset.UtcDateTime;

            string? hash = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;
            string? email = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? tokenType = principal?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;

            return (DateTime.UtcNow < lifetimeDateTime &&
                user.PasswordHash == hash &&
                user.Email == email &&
                tokenType == type);
        }
    }
}
