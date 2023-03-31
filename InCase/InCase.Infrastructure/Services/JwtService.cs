using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
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

        public string? GetIdFromRefreshToken(string refreshToken)
        {
            //Get claims
            byte[] secretBytes = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);
            ClaimsPrincipal? principal = GetClaimsToken(refreshToken, secretBytes);

            if (principal is null)
                return null;

            //Get user id TODO Cut in method
            string getUserId = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return getUserId;
        }

        public static ClaimsPrincipal? GetClaimsToken(string token, byte[] secret)
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

            string? lifetime = principal?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            DateTimeOffset lifetimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(lifetime ?? "0"));
            DateTime lifetimeDateTime = lifetimeOffset.UtcDateTime;

            if (DateTime.UtcNow >= lifetimeDateTime || 
                securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals("HS512",
                StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public string CreateEmailToken(in User user)
        {
            Claim[] claims = {
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserEmail", user.Email!),
            };

            TimeSpan expiration = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:EmailTokenValidityInMinutes"]!));

            string secret = user.PasswordHash + user.Email;

            JwtSecurityToken token = GenerateToken(claims, secret, expiration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DataSendTokens CreateTokenPair(in User user)
        {
            Claim[] claimsAccess = GenerateAccessTokenClaims(user);
            Claim[] claimsRefresh = {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            string secret = user.PasswordHash + user.Email;

            JwtSecurityToken accessToken = GenerateToken(claimsAccess, secret, expirationAccess);
            JwtSecurityToken refreshToken = GenerateToken(claimsRefresh, secret, expirationRefresh);

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
            string secret,
            TimeSpan expiration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(secret + _configuration["JWT:Secret"]!));
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
    }
}
