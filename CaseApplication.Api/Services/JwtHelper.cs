using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
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

        public JwtSecurityToken CreateEmailToken(
            Claim[] claims,
            string secret)
        {
            TimeSpan expiration = TimeSpan.FromMinutes(5);

            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(secret + _configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            return CreateToken(claims, credentials, expiration);
        }

        public string GenerateEmailToken(Guid userId, string hash)
        {
            Claim[] claims = {
                new Claim("UserId", userId.ToString())
            };

            JwtSecurityToken token = CreateEmailToken(claims, hash);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenModel GenerateTokenPair(in User user)
        {
            Claim[] claimsAccess = GenerateClaimsForAccessToken(user);
            Claim[] claimsRefresh = {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:TokenValidityInMinutes"]!));

            JwtSecurityToken accessToken = CreateResuableToken(claimsAccess, expirationAccess);
            JwtSecurityToken refreshToken = CreateResuableToken(claimsRefresh, expirationRefresh);

            return new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAccessIn = accessToken.ValidTo,
                ExpiresRefreshIn = refreshToken.ValidTo,
            };
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
        private Claim[] GenerateClaimsForAccessToken(User user)
        {
            //Find future data for claims
            Guid roleId = user.UserAdditionalInfo!.UserRoleId;

            string roleName = user.UserAdditionalInfo.UserRole!.RoleName!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.UserLogin!),
                new Claim(ClaimTypes.Email, user.UserEmail!)
            };
        }
    }
}
