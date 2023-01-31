using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAdditionalInfoRepository _userAdditionalInfoRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            IUserRepository userRepository, 
            IUserAdditionalInfoRepository userAdditionalInfoRepository,
            IUserRoleRepository userRoleRepository,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userAdditionalInfoRepository = userAdditionalInfoRepository;
            _userRoleRepository = userRoleRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Authenticate(User user, string password)
        {
            User searchUser = await _userRepository.GetByParameters(user);

            string hash = _encryptorHelper.EncryptorPassword(password, 
                Convert.FromBase64String(searchUser.PasswordSalt!));

            if (hash != searchUser.PasswordHash) 
                throw new Exception("Invalid login/email/id or password");

            List<Claim> claims = await GetClaims(user);

            TimeSpan expirationJwt = TimeSpan.FromMinutes(
                double.Parse(_configuration["CaseApp:TokenLifetime"]!));

            JwtSecurityToken token = _jwtHelper.GenerateJwt(expirationJwt, claims.ToArray());

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }

        [HttpPost]
        public async Task<bool> Logout(User user)
        {
            return true;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            //Find future data for claims
            List<Claim> claims = new List<Claim>();
            UserAdditionalInfo userAdditionalInfo = await _userAdditionalInfoRepository.Get(user.Id);

            Guid roleId = userAdditionalInfo.UserRoleId;
            string roleName = (await _userRoleRepository.Get(roleId)).RoleName!;

            //Add claims
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            claims.Add(new Claim(ClaimTypes.Name, user.UserLogin!));
            claims.Add(new Claim(ClaimTypes.Email, user.UserEmail!));
            claims.Add(new Claim(ClaimTypes.Hash, user.PasswordHash!));

            return claims;
        }
    }
}
