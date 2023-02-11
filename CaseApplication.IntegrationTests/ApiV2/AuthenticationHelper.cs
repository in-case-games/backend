using Azure.Core;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class AuthenticationHelper
    {
        private  IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();
        private readonly JwtHelper _jwtHelper;
        public AuthenticationHelper()
        {
            _jwtHelper = new(_configuration);
        }
        public string CreateToken()
        {
            UserAdditionalInfo info = new UserAdditionalInfo
            {
                UserRole = new UserRole { RoleName = "admin" }
            };
            User user = new User
            {
                Id = Guid.NewGuid(),
                UserLogin = "UserForTests",
                UserEmail = "UserEmailForTest",
                UserAdditionalInfo = info
            };

            Claim[] claims = GenerateClaimsForAccessToken(user);
            TimeSpan expiration = TimeSpan.FromMinutes(5);

            JwtSecurityToken token = _jwtHelper.CreateResuableToken(claims, expiration);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
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
