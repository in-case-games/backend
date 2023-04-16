using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InCase.Infrastructure.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ClaimsPrincipal? GetClaimsToken(string token)
        {
            byte[] secretBytes = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();

            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals("HS512",
                StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public string CreateEmailToken(in User user)
        {
            Claim[] claims = GenerateTokenClaims(in user, "email");

            TimeSpan expiration = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:EmailTokenValidityInMinutes"]!));

            JwtSecurityToken token = GenerateToken(claims, expiration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DataSendTokens CreateTokenPair(in User user)
        {
            Claim[] claimsAccess = GenerateAccessTokenClaims(in user);
            Claim[] claimsRefresh = GenerateTokenClaims(in user, "refresh");

            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            JwtSecurityToken accessToken = GenerateToken(claimsAccess, expirationAccess);
            JwtSecurityToken refreshToken = GenerateToken(claimsRefresh, expirationRefresh);

            return new DataSendTokens
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAccess = accessToken.ValidTo,
                ExpiresRefresh = refreshToken.ValidTo,
            };
        }

        private JwtSecurityToken GenerateToken(
            Claim[] claims,
            TimeSpan expiration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            return new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                claims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);
        }

        private static Claim[] GenerateAccessTokenClaims(in User user)
        {
            //Find future data for claims
            string roleName = user.AdditionalInfo!.Role!.Name!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.Login!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
        }

        private static Claim[] GenerateTokenClaims(in User user, string type)
        {
            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Hash, user.PasswordHash!),
                new Claim("TokenType", type)
            };
        }
    }
}
