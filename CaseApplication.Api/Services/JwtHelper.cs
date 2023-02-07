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

        public JwtSecurityToken CreateResuableToken(Claim[] claims, TimeSpan expiration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            return CreateToken(claims, credentials, expiration);
        }

        public JwtSecurityToken CreateOneTimeToken(
            Claim[] claims,
            string secret)
        {
            TimeSpan expiration = TimeSpan.FromMinutes(5);

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            return CreateToken(claims, credentials, expiration);
        }

        public ClaimsPrincipal? GetClaimsToken(
            string token,
            byte[] secret,
            string securityAlgorithm)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token, 
                tokenValidationParameters, 
                out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(securityAlgorithm, 
                StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        private JwtSecurityToken CreateToken(
            Claim[] claims,
            SigningCredentials credentials,
            TimeSpan expiration)
        {
            return new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                claims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);
        }
    }
}
