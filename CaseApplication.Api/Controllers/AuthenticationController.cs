using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IUserAdditionalInfoRepository _userAdditionalInfoRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserTokensRepository _userTokensRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        #endregion
        #region ctor
        public AuthenticationController(
            IConfiguration configuration,
            IUserRepository userRepository, 
            IUserAdditionalInfoRepository userAdditionalInfoRepository,
            IUserRoleRepository userRoleRepository,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            IUserTokensRepository userTokensRepository)
        {
            _userRepository = userRepository;
            _userAdditionalInfoRepository = userAdditionalInfoRepository;
            _userRoleRepository = userRoleRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _userTokensRepository = userTokensRepository;
            _configuration = configuration;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(User user, string password, string ip)
        {
            User? searchUser = await _userRepository.GetByParameters(user);

            if (searchUser is null) return Unauthorized();

            string hash = _encryptorHelper.EncryptorPassword(password,
                Convert.FromBase64String(searchUser.PasswordSalt!));

            if (hash != searchUser.PasswordHash) return Unauthorized();

            Claim[] claimsAccessToken = await GetClaimsAccessToken(searchUser);
            Claim[] claimsRefreshToken = {
                new Claim(ClaimTypes.NameIdentifier, searchUser.Id.ToString())
            };

            TokenModel tokenModel = CreateTokenPair(claimsAccessToken, claimsRefreshToken);
            List<UserToken> userTokens = await _userTokensRepository.GetAll(searchUser.Id);
            UserToken? userTokenByIp = userTokens.FirstOrDefault(x => x.UserIpAddress == ip);

            if (userTokenByIp == null)
            {
                await _userTokensRepository.Create(new UserToken()
                {
                    RefreshToken = tokenModel.RefreshToken,
                    RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn,
                    UserId = searchUser.Id,
                    UserIpAddress = ip,
                });
            }
            else
            {
                userTokenByIp.RefreshToken = tokenModel.RefreshToken;
                userTokenByIp.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
                await _userTokensRepository.Update(userTokenByIp);
            }

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(User user, string password)
        {
            user.Id = new Guid();

            User? userExists = await _userRepository.GetByParameters(user);

            if (userExists is not null) return BadRequest("User already exists!");

            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            await _userRepository.Create(user);

            user = (await _userRepository.GetByLogin(user.UserLogin!))!;

            await _userAdditionalInfoRepository.Create(new UserAdditionalInfo()
            {
                UserId = user.Id,
            });

            return Ok();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken, string ip)
        {
            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(refreshToken, _configuration["JWT:Secret"]!);

            if(principal is null)
                return BadRequest("Invalid refresh token");

            string getUserId = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            _ = Guid.TryParse(getUserId, out Guid userId);

            UserToken? userToken = await _userTokensRepository.GetByIp(userId, ip);

            if (userToken == null ||
                refreshToken != userToken.RefreshToken || 
                userToken.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid refresh token");
            }

            User user = (await _userRepository.Get(userId))!;

            Claim[] claimsAccess = await GetClaimsAccessToken(user);
            Claim[] claimsRefresh = principal.Claims.ToArray();

            TokenModel tokenModel = CreateTokenPair(claimsAccess, claimsRefresh);

            userToken.RefreshToken = tokenModel.RefreshToken;
            userToken.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;

            await _userTokensRepository.Update(userToken);

            return Ok(tokenModel);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string ip)
        {
            UserToken? userToken = await _userTokensRepository.GetByIp(UserId, ip);
            
            await _userTokensRepository.Delete(userToken!.Id);
            
            return NoContent();
        }

        [Authorize]
        [HttpPost("LogoutAll")]
        public async Task<IActionResult> LogoutAll()
        {
            await _userTokensRepository.DeleteAll(UserId);

            return NoContent();
        }

        private async Task<Claim[]> GetClaimsAccessToken(User user)
        {
            //Find future data for claims
            UserAdditionalInfo? userAdditionalInfo = await _userAdditionalInfoRepository.Get(user.Id);

            Guid roleId = userAdditionalInfo!.UserRoleId;
            string roleName = (await _userRoleRepository.Get(roleId))!.RoleName!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.UserLogin!),
                new Claim(ClaimTypes.Email, user.UserEmail!)
            };
        }

        private TokenModel CreateTokenPair(Claim[] claimsAccess, Claim[] claimsRefresh)
        {
            TimeSpan expirationRefresh = TimeSpan.FromDays(
                double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:TokenValidityInMinutes"]!));

            JwtSecurityToken accessToken = _jwtHelper
                .CreateResuableToken(claimsAccess, expirationAccess);
            JwtSecurityToken refreshToken = _jwtHelper
                .CreateResuableToken(claimsRefresh, expirationRefresh);

            return new TokenModel { 
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                ExpiresAccessIn = accessToken.ValidTo,
                ExpiresRefreshIn = refreshToken.ValidTo,
        };
        }
    }
}
