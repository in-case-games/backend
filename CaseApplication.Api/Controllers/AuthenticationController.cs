using AutoMapper;
using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
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
        private MapperConfiguration _mapperConfigurationInfo = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfo, UserAdditionalInfoDto>();
        });
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
        public async Task<IActionResult> SignIn(UserDto user, string password, string ip)
        {
            //FindUser
            User? searchUser = await _userRepository.GetByParameters(user);

            if (searchUser is null) return NotFound();
            if (_validationService.IsValidUserPassword(in searchUser, password) is false) 
                return Forbid();

            //Search refresh token by ip
            UserToken? userTokenByIp = searchUser.UserTokens!.FirstOrDefault(x => x.UserIpAddress == ip);

            if (userTokenByIp == null)
            {
                await _emailHelper.SendConfirmAccountToEmail(new EmailModel()
                {
                    UserEmail = searchUser.UserEmail!,
                    UserId = searchUser.Id,
                    UserToken = _jwtHelper.GenerateEmailToken(searchUser, ip),
                    UserIp = ip
                });

                return Unauthorized("Check email");
            }

            //Generation Token
            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in searchUser);

            MapUserTokenForUpdate(ref userTokenByIp, tokenModel);
            await _userTokensRepository.Update(userTokenByIp);

            await _emailHelper.SendNotifyToEmail(
                searchUser.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вход в аккаунт"
                });

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("signup/{password}")]
        public async Task<IActionResult> SignUp(UserDto user, string password)
        {
            //Find user
            User? userExists = await _userRepository.GetByParameters(user);

            if (userExists is not null) return Conflict("User already exists!");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();

            user.PasswordHash = _encryptorHelper.EncryptorPassword(password, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            await _userRepository.Create(user);

            //Create Add info
            User createdUser = (await _userRepository.GetByLogin(user.UserLogin!))!;

            await _userAdditionalInfoRepository.Create(new()
            {
                UserId = createdUser.Id,
            });

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Поздравляем о регистрации"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("refresh/{refreshToken}&{ip}")]
        public async Task<IActionResult> RefreshTokens(string refreshToken, string ip)
        {
            //Get User id
            string? getUserId = _jwtHelper.GetIdFromRefreshToken(refreshToken);

            if(getUserId == null) return Forbid("Invalid refresh token");

            _ = Guid.TryParse(getUserId, out Guid userId);

            //Search refresh token by ip TODO Cut in method
            User user = (await _userRepository.Get(userId))!;
            UserToken? userToken = user.UserTokens!.FirstOrDefault(x => x.UserIpAddress == ip);

            if (_validationService.IsValidRefreshToken(in userToken, refreshToken))
            {
                //Generate token
                TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in user);

                MapUserTokenForUpdate(ref userToken!, tokenModel);
                await _userTokensRepository.Update(userToken);

                return Ok(tokenModel);
            }

            await _emailHelper.SendNotifyToEmail(user.UserEmail!, "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Попытка входа в аккаунт"
                });

            await _userTokensRepository.DeleteByToken(userId, refreshToken);

            return Forbid("Invalid refresh token");
        }

        [AllowAnonymous]
        [HttpGet("confirm/{userId}&{token}&{ip}")]
        public async Task<IActionResult> ConfirmAccount(Guid userId, string token, string ip)
        {
            //TODO OneTimeToken
            EmailModel emailModel = new()
            {
                UserId = userId,
                UserToken = token,
                UserIp = ip
            };
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            bool IsTokenUsed = user.UserTokens!.FirstOrDefault(
                x => x.UserIpAddress == emailModel.UserIp) is not null;
            
            if (IsTokenUsed) return Forbid("Invalid token used");

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;

            if (userInfo.IsConfirmedAccount == false)
            {
                userInfo.IsConfirmedAccount = true;

                await _userAdditionalInfoRepository.Update(userInfo);

                await _emailHelper.SendNotifyToEmail(
                    user.UserEmail!,
                    "Администрация сайта",
                    new EmailPatternModel()
                    {
                        Body = $"Спасибо что подтвердили аккаунт"
                    });
            }

            //Generate tokens
            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in user);

            UserToken newUserToken = new()
            {
                UserId = user.Id,
                UserIpAddress = emailModel.UserIp,
            };

            MapUserTokenForUpdate(ref newUserToken, tokenModel);

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вход в аккаунт"
                });

            await _userTokensRepository.Create(newUserToken);

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

        private static void MapUserTokenForUpdate(ref UserToken userToken, TokenModel tokenModel)
        {
            userToken.RefreshToken = tokenModel.RefreshToken;
            userToken.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
            userToken.RefreshTokenCreationTime = DateTime.UtcNow;
        }
    }
}
