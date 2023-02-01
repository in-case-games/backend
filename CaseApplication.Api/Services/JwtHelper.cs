using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Api.Services
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken GenerateJwt(TimeSpan expiration, Claim[] additionalClaims)
        {
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(
                _configuration["CaseApp:JWTKey"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                _configuration["CaseApp:Issuer"],
                _configuration["CaseApp:Audience"]!,
                additionalClaims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);

            return token;
        }
    }
}
