using Authentication.BLL.Exceptions;
using Authentication.DAL.Entities;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Authentication.BLL.Services
{
    public static class ValidationService
    {
        public static bool IsValidUserPassword(in User user, string? password)
        {
            password ??= string.Empty;

            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public static void CheckCorrectPassword(string? password)
        {
            var regex = new Regex(@"(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[a-z])");

            if (password is null || password.Length <= 4 || password.Length > 50) 
                throw new BadRequestException("Пароль должен быть в длину от 5 до 50 символов");
            if (regex.IsMatch(password)) 
                throw new BadRequestException("Пароль должен содержать цифру, заглавную и строчную букву");
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
