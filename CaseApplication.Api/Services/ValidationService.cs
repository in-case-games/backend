using CaseApplication.DomainLayer.Entities;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Api.Services
{
    public class ValidationService
    {
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public ValidationService(
            EncryptorHelper encryptorHelper, 
            JwtHelper jwtHelper,
            IConfiguration configuration)    
        {
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        public bool IsValidUserPassword(in User user, string password)
        {
            string hash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public bool IsValidEmailToken(string token, string hash)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(hash + _configuration["JWT:Secret"]!);

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(token, secretBytes, "HS512");

            return (principal is not null);
        }
    }
}
