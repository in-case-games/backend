using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        ///<summary>
        /// Reads and validates a 'JSON Web Token' (JWT) and get claims
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <exception cref="UnauthorizedCodeException"><paramref name="token"/>Is incorrect or invalid</exception>
        /// <returns>A <see cref="ClaimsPrincipal"/> from the JWT. Does not include claims found in the JWT header.</returns>
        public ClaimsPrincipal GetClaimsToken(string token)
        {
            var secret = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);
            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = false
            };
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var claims = handler.ValidateToken(token, parameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals("HS512", StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException();

                return claims;
            }
            catch (SecurityTokenException)
            {
                throw new UnauthorizedException("Не валидный токен");
            }
            catch (ArgumentException)
            {
                throw new UnauthorizedException("Не валидный токен");
            }
        }

        public string CreateEmailToken(in User user)
        {
            var claims = GenerateTokenClaims(in user, "email");
            var expiration = TimeSpan.FromMinutes(double.Parse(_configuration["JWT:EmailTokenValidityInMinutes"]!));
            var token = GenerateToken(claims, expiration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokensResponse CreateTokenPair(in User user)
        {
            if(string.IsNullOrEmpty(user.AdditionalInfo?.Role?.Name))
                throw new ArgumentNullException("user.RoleName", "The role of the user when creating the token is mandatory");

            var accessToken = GenerateToken(GenerateAccessTokenClaims(in user), 
                TimeSpan.FromMinutes(double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!)));
            var refreshToken = GenerateToken(GenerateTokenClaims(in user, "refresh"), 
                TimeSpan.FromDays(double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!)));

            return new TokensResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAccess = accessToken.ValidTo,
                ExpiresRefresh = refreshToken.ValidTo,
            };
        }

        private JwtSecurityToken GenerateToken(Claim[] claims, TimeSpan expiration) =>
            new(_configuration["JWT:ValidIssuer"], 
                _configuration["JWT:ValidAudience"]!,
                claims, 
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!)),
                        SecurityAlgorithms.HmacSha512));

        private static Claim[] GenerateAccessTokenClaims(in User user) => 
            new Claim[] {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.AdditionalInfo!.Role!.Name!),
                new(ClaimTypes.Name, user.Login!),
                new(ClaimTypes.Email, user.Email!)
            };

        private static Claim[] GenerateTokenClaims(in User user, string type) =>
            new Claim[] {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Hash, user.PasswordHash!),
                new("TokenType", type)
            };
    }
}
