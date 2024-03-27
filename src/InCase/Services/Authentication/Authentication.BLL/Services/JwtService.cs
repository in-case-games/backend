using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.BLL.Models;

namespace Authentication.BLL.Services;
public class JwtService(IConfiguration configuration) : IJwtService
{
    private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    private readonly double _emailTokenValidityInMinutes = double.Parse(configuration[$"JWT:EmailTokenValidityInMinutes:{Env}"]!);
    private readonly double _accessTokenValidityInMinutes = double.Parse(configuration[$"JWT:AccessTokenValidityInMinutes:{Env}"]!);
    private readonly double _refreshTokenValidityInDays = double.Parse(configuration[$"JWT:RefreshTokenValidityInDays:{Env}"]!);
    private readonly string? _validIssuer = configuration[$"JWT:ValidIssuer:{Env}"];
    private readonly string? _validAudience = configuration[$"JWT:ValidAudience:{Env}"];
    private readonly byte[] _jwtSecret = Encoding.ASCII.GetBytes(configuration[$"JWT:Secret:{Env}"]!);

    /// <summary>
    ///  Reads and validates a 'JSON Web Token' (JWT) and get claims
    ///  </summary>
    ///  <param name="token">JWT token</param>
    ///  <exception>
    ///      <cref>UnauthorizedCodeException</cref>
    ///      <paramref name="token"/>Is incorrect or invalid</exception>
    ///  <returns>A <see cref="ClaimsPrincipal"/> from the JWT. Does not include claims found in the JWT header.</returns>
    public ClaimsPrincipal GetClaimsToken(string token)
    {
        var secret = Encoding.ASCII.GetBytes(configuration[$"JWT:Secret:{Env}"]!);
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
            var claims = handler.ValidateToken(token, parameters, out var securityToken);

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
        var expiration = TimeSpan.FromMinutes(_emailTokenValidityInMinutes);
        var token = GenerateToken(claims, expiration);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokensResponse CreateTokenPair(in User user)
    {
        if(string.IsNullOrEmpty(user.AdditionalInfo?.Role?.Name))
            throw new ArgumentNullException(nameof(user), 
                "The role of the user when creating the token is mandatory");

        var accessToken = GenerateToken(GenerateAccessTokenClaims(in user), TimeSpan.FromMinutes(_accessTokenValidityInMinutes));
        var refreshToken = GenerateToken(GenerateTokenClaims(in user, "refresh"), TimeSpan.FromDays(_refreshTokenValidityInDays));

        return new TokensResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
            ExpiresAccess = accessToken.ValidTo,
            ExpiresRefresh = refreshToken.ValidTo,
        };
    }

    private JwtSecurityToken GenerateToken(Claim[] claims, TimeSpan expiration)
    {
        var credentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSecret), SecurityAlgorithms.HmacSha512);

        return claims == null ? 
            throw new ArgumentNullException(nameof(claims)) : 
            new JwtSecurityToken(_validIssuer, _validAudience, claims, 
                                 expires: DateTime.UtcNow.Add(expiration), 
                                 signingCredentials: credentials);
    }

    private static Claim[] GenerateAccessTokenClaims(in User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.AdditionalInfo!.Role!.Name!),
        new Claim(ClaimTypes.Name, user.Login!),
        new Claim(ClaimTypes.Email, user.Email!)
    ];

    private static Claim[] GenerateTokenClaims(in User user, string type) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Hash, user.PasswordHash!),
        new Claim("TokenType", type)
    ];
}