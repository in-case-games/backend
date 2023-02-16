using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using Microsoft.AspNetCore.Authentication;
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

        public bool IsValidEmailToken(in EmailModel emailModel, in User user)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(user.PasswordHash! + _configuration["JWT:Secret"]!);
            string token = emailModel.EmailToken;

            bool IsTokenUsed = user.UserTokens!.Any(x => x.EmailToken == token);

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(token, secretBytes, "HS512");

            return principal is not null && IsTokenUsed is false;
        }

        public bool IsValidEmailTokenSend(in User user, string ip, string password)
        {
            UserToken? userToken = user.UserTokens!.FirstOrDefault(
                x => x.UserIpAddress == ip);

            bool isValidPassword = IsValidUserPassword(in user, password);

            return (userToken != null && isValidPassword);
        }
    }
}
