using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IUserAdditionalInfoRepository _userAdditionalInfoRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserTokensRepository _userTokensRepository;
        private readonly EmailHelper _emailHelper;
        private readonly ValidationService _validationService;
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
            IUserTokensRepository userTokensRepository,
            EmailHelper emailHelper,
            ValidationService validationService)
        {
            _userRepository = userRepository;
            _userAdditionalInfoRepository = userAdditionalInfoRepository;
            _userRoleRepository = userRoleRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _userTokensRepository = userTokensRepository;
            _configuration = configuration;
            _emailHelper = emailHelper;
            _validationService = validationService;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("signin/{password}&{ip}")]
        public async Task<IActionResult> SignIn(User user, string password, string ip)
        {
            //FindUser
            User? searchUser = await _userRepository.GetByParameters(user);

            if (searchUser is null) return NotFound();
            if (_validationService.IsValidUserPassword(in searchUser, password) is false) 
                return Forbid();

            //Search refresh token by ip
            UserToken? userTokenByIp = await _userTokensRepository.GetByIp(searchUser.Id, ip);

            if (userTokenByIp == null)
            {
                string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

                await _emailHelper.SendConfirmAccountToEmail(new EmailModel()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    UserToken = emailToken,
                    UserIp = ip
                });

                return Unauthorized("Check email");
            }

            //Generate tokens TODO Cut in method
            Claim[] claimsAccessToken = await GetClaimsForAccessToken(searchUser);
            Claim[] claimsRefreshToken = {
                new Claim(ClaimTypes.NameIdentifier, searchUser.Id.ToString())
            };

            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(claimsAccessToken, claimsRefreshToken);

            userTokenByIp.RefreshToken = tokenModel.RefreshToken;
            userTokenByIp.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
            userTokenByIp.RefreshTokenCreationTime = DateTime.UtcNow;
            
            await _userTokensRepository.Update(userTokenByIp);

            await _emailHelper.SendNotifyAccountSignIn(user.UserEmail!);

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("signup/{password}")]
        public async Task<IActionResult> SignUp(User user, string password)
        {
            //Find user
            User? userExists = await _userRepository.GetByParameters(user);

            if (userExists is not null) return Conflict("User already exists!");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            await _userRepository.Create(user);

            //Create Add info
            user = (await _userRepository.GetByLogin(user.UserLogin!))!;

            await _userAdditionalInfoRepository.Create(new UserAdditionalInfo()
            {
                UserId = user.Id,
            });

            await _emailHelper.SendNotifyAccountSignUp(user.UserEmail!);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("refresh/{refreshToken}&{ip}")]
        public async Task<IActionResult> RefreshTokens(string refreshToken, string ip)
        {
            //Get claims by refresh token TODO Cut in method
            byte[] secretBytes = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);
            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(refreshToken, secretBytes, "HS256");

            if (principal is null)
                return Forbid("Invalid refresh token");

            //Get user id TODO Cut in method
            string getUserId = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            _ = Guid.TryParse(getUserId, out Guid userId);

            //Search refresh token by ip TODO Cut in method
            User user = (await _userRepository.Get(userId))!;
            UserToken? userToken = await _userTokensRepository.GetByIp(userId, ip);

            if(userToken == null)
            {
                await _emailHelper.SendNotifyAttemptSingIn(user.UserEmail!);
            }

            if (userToken == null ||
                refreshToken != userToken.RefreshToken || 
                userToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                await _userTokensRepository.DeleteByToken(userId, refreshToken);
                return Forbid("Invalid refresh token");
            }

            //Generation Token TODO Cut in method
            Claim[] claimsAccess = await GetClaimsForAccessToken(user);
            Claim[] claimsRefresh = principal.Claims.ToArray();

            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(claimsAccess, claimsRefresh);

            userToken.RefreshToken = tokenModel.RefreshToken;
            userToken.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
            userToken.RefreshTokenCreationTime = DateTime.UtcNow;

            await _userTokensRepository.Update(userToken);

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccount(EmailModel emailModel)
        {
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();
            if (string.IsNullOrEmpty(emailModel.UserIp)) return Forbid("Invalid ip");
            if (_validationService.IsValidEmailToken(emailModel.UserToken, user.PasswordHash!) is false)
                return Forbid("Invalid email token");

            UserAdditionalInfo? userInfo = await _userAdditionalInfoRepository
                .GetByUserId(emailModel.UserId);

            if(userInfo!.IsConfirmedAccount == false)
            {
                userInfo!.IsConfirmedAccount = true;

                await _userAdditionalInfoRepository.Update(userInfo);
                await _emailHelper.SendNotifyActivateEmail(user.UserEmail!);
            }

            //Generate tokens TODO Cut in method
            Claim[] claimsAccessToken = await GetClaimsForAccessToken(user);
            Claim[] claimsRefreshToken = {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(claimsAccessToken, claimsRefreshToken);

            await _userTokensRepository.Create(new UserToken()
            {
                RefreshToken = tokenModel.RefreshToken,
                RefreshTokenCreationTime = DateTime.UtcNow,
                RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn,
                UserId = user.Id,
                UserIpAddress = emailModel.UserIp,
            });

            await _emailHelper.SendNotifyAccountSignIn(user.UserEmail!);

            return Ok(tokenModel);
        }

        [Authorize]
        [HttpDelete("{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            UserToken? userToken = await _userTokensRepository.GetByToken(UserId, refreshToken);

            if (userToken == null) return Forbid("Invalid token");

            await _userTokensRepository.Delete(userToken.Id);
            
            return NoContent();
        }

        [Authorize]
        [HttpDelete("all/{refreshToken}")]
        public async Task<IActionResult> LogoutAll(string refreshToken)
        {
            UserToken? userToken = await _userTokensRepository.GetByToken(UserId, refreshToken);

            if (userToken == null) return Forbid("Invalid token");

            await _userTokensRepository.DeleteAll(UserId);

            return NoContent();
        }

        private async Task<Claim[]> GetClaimsForAccessToken(User user)
        {
            //Find future data for claims
            UserAdditionalInfo? userAdditionalInfo = await _userAdditionalInfoRepository
                .GetByUserId(user.Id);

            Guid roleId = userAdditionalInfo!.UserRoleId;
            string roleName = (await _userRoleRepository.Get(roleId))!.RoleName!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.UserLogin!),
                new Claim(ClaimTypes.Email, user.UserEmail!)
            };
        }
    }
}
