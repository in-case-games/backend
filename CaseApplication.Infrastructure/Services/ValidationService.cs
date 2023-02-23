using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Infrastructure.Services
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
            string hash = EncryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public bool IsValidEmailToken(in DataMailLink emailModel, in User user)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(user.PasswordHash! + _configuration["JWT:Secret"]!);
            string token = emailModel.EmailToken;

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(token, secretBytes, "HS512");

            if (principal is null) return false;

            string email = principal.Claims.FirstOrDefault(x => x.ValueType == "UserEmail")!.Value;

            bool IsNoChangeEmail = email == user.UserEmail;
            bool IsNoUsed = user.UserTokens!.Any(x => x.EmailToken == token) is false;

            return IsNoUsed && IsNoChangeEmail;
        }
    }
}
