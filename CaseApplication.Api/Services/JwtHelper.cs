using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public JwtSecurityToken CreateAccessToken(Claim[] additionalClaims)
        {
            TimeSpan expiration = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:TokenValidityInMinutes"]!));

            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(
                _configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                additionalClaims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);

            return token;
        }

        public JwtSecurityToken CreateRefreshToken(Claim[] additionalClaims)
        {
            TimeSpan expiration = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));

            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(
                _configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                additionalClaims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);

            return token;
        }

        public JwtSecurityToken CreateOneTimeToken(
            Claim[] additionalClaims,
            string secret)
        {
            TimeSpan expiration = TimeSpan.FromMinutes(5);

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                additionalClaims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);

            return token;
        }

        public ClaimsPrincipal? GetClaimsOneTimeToken(
            string token,
            string secret)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public ClaimsPrincipal? GetClaimsToken(
            string token,
            string secret)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(secret)),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token, 
                tokenValidationParameters, 
                out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
    }
}
