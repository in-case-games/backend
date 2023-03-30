using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace InCase.Infrastructure.Services
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

        public bool IsValidToken(string token, in User user)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(user.PasswordHash! + _configuration["JWT:Secret"]!);

            ClaimsPrincipal? principal = JwtService.GetClaimsToken(token, secretBytes, "HS512");
            string? lifetime = principal?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            DateTimeOffset lifetimeOffSet = DateTimeOffset.FromUnixTimeSeconds(long.Parse(lifetime ?? "0"));
            DateTime lifeTimeDateTime = lifetimeOffSet.UtcDateTime;

            return (DateTime.UtcNow < lifeTimeDateTime);
        }
    }
}
