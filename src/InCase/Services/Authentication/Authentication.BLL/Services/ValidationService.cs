using Authentication.BLL.Exceptions;
using Authentication.DAL.Entities;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Authentication.BLL.Services
{
    public static class ValidationService
    {
        private static readonly Regex Regex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$");

        public static bool IsValidUserPassword(in User user, string? password) =>
            EncryptorService.GenerationHashSha512(
                password ?? "", 
                    Convert.FromBase64String(user.PasswordSalt!)) == user.PasswordHash;

        public static void CheckCorrectPassword(string? password)
        {
            if (password is null || password.Length <= 4 || password.Length > 50) 
                throw new BadRequestException("Пароль должен быть в длину от 5 до 50 символов");
            if (!Regex.IsMatch(password)) 
                throw new BadRequestException("Пароль должен содержать цифру, заглавную и строчную букву");
        }

        public static bool IsValidToken(in User user, ClaimsPrincipal principal, string type) => 
            DateTime.UtcNow < DateTimeOffset.FromUnixTimeSeconds(long.Parse(
                principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0")
            ).UtcDateTime &&
            user.Email == principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value &&
            type == principal?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;
    }
}
