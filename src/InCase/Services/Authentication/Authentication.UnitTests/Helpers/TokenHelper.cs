using Authentication.BLL;
using Authentication.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.UnitTests.Helpers
{
    public class TokenHelper
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();
        public static JwtSecurityToken GenerateToken(
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
        public static string CreateEmailToken(in User user)
        {
            Claim[] claims = GenerateTokenClaims(in user, "email");

            TimeSpan expiration = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:EmailTokenValidityInMinutes"]!));

            JwtSecurityToken token = GenerateToken(claims, expiration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static TokensResponse CreateTokenPair(in User user)
        {
            if (string.IsNullOrEmpty(user.AdditionalInfo?.Role?.Name))
                throw new ArgumentNullException("Role name",
                    "The role of the user when creating the token is mandatory");

            Claim[] claimsAccess = GenerateAccessTokenClaims(in user);
            Claim[] claimsRefresh = GenerateTokenClaims(in user, "refresh");

            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            JwtSecurityToken accessToken = GenerateToken(claimsAccess, expirationAccess);
            JwtSecurityToken refreshToken = GenerateToken(claimsRefresh, expirationRefresh);

            return new TokensResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAccess = accessToken.ValidTo,
                ExpiresRefresh = refreshToken.ValidTo,
            };
        }

        public static string CreateToken(User user)
        {
            Claim[] claims = GenerateClaimsForAccessToken(user);
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            JwtSecurityToken token = GenerateToken(claims, expirationAccess);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Claim[] GenerateClaimsForAccessToken(User user)
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
        private static Claim[] GenerateAccessTokenClaims(in User user)
        {
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
