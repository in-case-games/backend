using System.Net.Mail;
using Authentication.BLL.Exceptions;
using Authentication.DAL.Entities;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Authentication.BLL.Services
{
    public static class ValidationService
    {
        private static readonly Regex PasswordRegex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$");

        public static bool IsValidUserPassword(in User user, string? password) =>
            EncryptorService.GenerationHashSha512(
                password ?? "", 
                    Convert.FromBase64String(user.PasswordSalt!)) == user.PasswordHash;

        public static void CheckCorrectPassword(string? password)
        {
            if (password is null || password.Length <= 4 || password.Length > 50) 
                throw new BadRequestException("Пароль должен быть в длину от 5 до 50 символов");
            if (!PasswordRegex.IsMatch(password)) 
                throw new BadRequestException("Пароль должен содержать цифру, заглавную и строчную букву");
        }

        public static bool CheckCorrectEmail(string? email)
        {
            if (!MailAddress.TryCreate(email, out var mailAddress)) return false;

            // And if you want to be more strict:
            var hostParts = mailAddress.Host.Split('.');

            if (hostParts.Length == 1) return false; // No dot.
            if (hostParts.Any(p => p == string.Empty)) return false; // Double dot.
            if (hostParts[^1].Length < 2) return false; // TLD only one letter.

            // Double dot or dot at end of user part.
            return !mailAddress.User.Contains(' ') && mailAddress.User.Split('.').All(p => p != string.Empty);
        }

        public static bool CheckCorrectLogin(string? login) => login is not null && login.Length >= 3;

        public static bool IsValidToken(in User user, ClaimsPrincipal principal, string type) => 
            DateTime.UtcNow < DateTimeOffset.FromUnixTimeSeconds(long.Parse(
                principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0")
            ).UtcDateTime &&
            user.Email == principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value &&
            type == principal?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;
    }
}
