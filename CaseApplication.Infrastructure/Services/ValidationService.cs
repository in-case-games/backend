using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Infrastructure.Services
{
    public class ValidationService
    {
        private readonly IConfiguration _configuration;

        public ValidationService(
            IConfiguration configuration)    
        {
            _configuration = configuration;
        }

        public static bool IsValidUserPassword(in User user, string password)
        {
            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public bool IsValidEmailToken(in DataMailLink emailModel, in User user)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(user.PasswordHash! + _configuration["JWT:Secret"]!);
            string token = emailModel.EmailToken;

            ClaimsPrincipal? principal = JwtService.GetClaimsToken(token, secretBytes, "HS512");

            if (principal is null) return false;

            string? email = principal.Claims.FirstOrDefault(x => x.Type == "UserEmail")?.Value;

            bool IsNoChangeEmail = email == user.UserEmail;
            bool IsNoUsed = user.UserTokens!.Any(x => x.EmailToken == token) is false;

            return IsNoUsed && IsNoChangeEmail;
        }
    }
}
