using CaseApplication.DomainLayer.Entities;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Api.Services
{
    public class ValidationService
    {
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;

        public ValidationService(
            EncryptorHelper encryptorHelper, 
            JwtHelper jwtHelper)
        {
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
        }

        public bool IsValidUserPassword(in User user, string password)
        {
            string hash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public bool IsValidEmailToken(string token, string hash)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(hash);

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(token, secretBytes, "HS512");

            return (principal is not null);
        }
    }
}
