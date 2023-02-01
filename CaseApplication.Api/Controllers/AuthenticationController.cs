using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IUserRepository _userRepository;
        private readonly IUserAdditionalInfoRepository _userAdditionalInfoRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;
        #endregion
        #region ctor
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
        #endregion
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(User user, string password)
        {
            User? searchUser = await _userRepository.GetByParameters(user);

            if (searchUser is null) return Unauthorized();

            string hash = _encryptorHelper.EncryptorPassword(password,
                Convert.FromBase64String(searchUser.PasswordSalt!));

            if (hash != searchUser.PasswordHash) return Unauthorized();

            List<Claim> claims = await GetClaims(searchUser);

            JwtSecurityToken token = _jwtHelper.CreateToken(claims.ToArray());

            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            });
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(User user, string password)
        {
            user.Id = new Guid();
            User? userExists = await _userRepository.GetByParameters(user);
            if (userExists is not null) return BadRequest("User already exists!");

            string salt;
            int countIteration = 10000;

            do
            {
                if (countIteration == 0) throw new Exception("Request exceeded the waiting time");

                salt = _encryptorHelper.GenerationSaltTo64Bytes();

                --countIteration;
            }
            while (!await _userRepository.IsUniqueSalt(salt));

            byte[] saltEncoding = Convert.FromBase64String(salt);
            string hash = _encryptorHelper.EncryptorPassword(password, saltEncoding);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            return Ok(await _userRepository.Create(user));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            ClaimsPrincipal? principal = _jwtHelper.GetPrincipalFromExpiredToken(accessToken);

            if(principal is null)
                return BadRequest("Invalid access token or refresh token");

            string userLogin = principal.Identity!.Name!;

            User? user = await _userRepository.GetByLogin(userLogin);

            if (
                user == null || 
                user.RefreshToken != refreshToken || 
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            JwtSecurityToken newAccessToken = _jwtHelper.CreateToken(principal.Claims.ToArray());
            string newRefreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userRepository.Update(user!);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("Logout")]
        public bool Logout(User user)
        {
            return true;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            //Find future data for claims
            List<Claim> claims = new();
            UserAdditionalInfo? userAdditionalInfo = await _userAdditionalInfoRepository.Get(user.Id);

            Guid roleId = userAdditionalInfo!.UserRoleId;
            string roleName = (await _userRoleRepository.Get(roleId))!.RoleName!;

            //Add claims
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            claims.Add(new Claim(ClaimTypes.Name, user.UserLogin!));
            claims.Add(new Claim("UserEmail", user.UserEmail!));
            claims.Add(new Claim("PasswordHash", user.PasswordHash!));

            return claims;
        }
    }
}
