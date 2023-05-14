using InCase.Infrastructure.Exceptions;
using InCase.Infrastructure.Models.Authentication.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.BLL.Services
{
    public class JwtService
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
            byte[] secret = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

            TokenValidationParameters parameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token, parameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals("HS512", StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException();

                return principal;
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
            Claim[] claims = GenerateTokenClaims(in user, "email");

            TimeSpan expiration = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:EmailTokenValidityInMinutes"]!));

            JwtSecurityToken token = GenerateToken(claims, expiration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenResponse CreateTokenPair(in User user)
        {
            if(string.IsNullOrEmpty(user?.AdditionalInfo?.Role?.Name))
                throw new ArgumentNullException("Role.Name", 
                    "The role of the user when creating the token is mandatory");

            Claim[] claimsAccess = GenerateAccessTokenClaims(in user);
            Claim[] claimsRefresh = GenerateTokenClaims(in user, "refresh");

            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            JwtSecurityToken accessToken = GenerateToken(claimsAccess, expirationAccess);
            JwtSecurityToken refreshToken = GenerateToken(claimsRefresh, expirationRefresh);

            return new RefreshTokenResponse
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
