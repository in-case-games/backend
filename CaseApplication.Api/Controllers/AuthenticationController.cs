using AutoMapper;
using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly EmailHelper _emailHelper;
        private readonly ValidationService _validationService;
        private MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfo, UserAdditionalInfoDto>();
            configuration.CreateMap<UserDto, User>();
        });
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        #endregion
        #region ctor
        public AuthenticationController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            EmailHelper emailHelper,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
            _validationService = validationService;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("signin/{password}&{ip}")]
        public async Task<IActionResult> SignIn(UserDto userDto, string password, string ip)
        {
            //FindUser
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.Id == userDto.Id ||
                x.UserLogin == userDto.UserLogin);

            if (user is null) return NotFound();
            if (_validationService.IsValidUserPassword(in user, password) is false) 
                return Forbid();

            //Search refresh token by ip
            UserToken? userTokenByIp = user.UserTokens?.FirstOrDefault(x => x.UserIpAddress == ip);

            if (userTokenByIp == null)
            {
                await _emailHelper.SendConfirmAccountToEmail(new EmailModel()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    UserToken = _jwtHelper.GenerateEmailToken(user, ip),
                    UserIp = ip
                });

                return Unauthorized("Check email");
            }

            //Generation Token
            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in user);

            MapUserTokenForUpdate(ref userTokenByIp, tokenModel);

            UserToken? oldToken = await context.UserToken.FirstOrDefaultAsync(x => x.Id == userTokenByIp.Id);

            if (oldToken == null)
            {
                throw new Exception("There is no such token, " +
                    "review what data comes from the api");
            }

            context.Entry(oldToken).CurrentValues.SetValues(userTokenByIp);
            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вход в аккаунт"
                });

            return Ok(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("signup/{password}")]
        public async Task<IActionResult> SignUp(UserDto userDto, string password)
        {
            //Find user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? userExists = await context
                .User
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.Id == userDto.Id ||
                x.UserLogin == userDto.UserLogin);

            if (userExists is not null) return Conflict("User already exists!");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();

            userDto.PasswordHash = _encryptorHelper.EncryptorPassword(password, salt);
            userDto.PasswordSalt = Convert.ToBase64String(salt);

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            User user = mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);

            //Create Add info
            User? createdUser = await context
               .User
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.UserLogin == user.UserLogin);

            UserAdditionalInfo info = new UserAdditionalInfo();

            UserRole? role = await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.RoleName == "user");

            info.Id = Guid.NewGuid();
            info.UserRoleId = role!.Id;
            info.UserId = user.Id!;

            await context.UserAdditionalInfo.AddAsync(info);
            await context.SaveChangesAsync();

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
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //Get User id
            string? getUserId = _jwtHelper.GetIdFromRefreshToken(refreshToken);

            if(getUserId == null) return Forbid("Invalid refresh token");

            _ = Guid.TryParse(getUserId, out Guid userId);

            //Search refresh token by ip TODO Cut in method
            User? user = await context
                .User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            UserToken? userToken = user!.UserTokens?.FirstOrDefault(x => x.UserIpAddress == ip);

            if (_validationService.IsValidRefreshToken(in userToken, refreshToken))
            {
                //Generate token
                TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in user);

                MapUserTokenForUpdate(ref userToken!, tokenModel);

                context.Entry(refreshToken).CurrentValues.SetValues(userToken);

                return Ok(tokenModel);
            }

            await _emailHelper.SendNotifyToEmail(user.UserEmail!, "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Попытка входа в аккаунт"
                });

            UserToken? refreshModelToken = await context.UserToken
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

            context.UserToken.Remove(refreshModelToken!);
            await context.SaveChangesAsync();

            return Forbid("Invalid refresh token");
        }

        [AllowAnonymous]
        [HttpGet("confirm/{userId}&{token}&{ip}")]
        public async Task<IActionResult> ConfirmAccount(Guid userId, string token, string ip)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            //TODO OneTimeToken
            EmailModel emailModel = new()
            {
                UserId = userId,
                UserToken = token,
                UserIp = ip
            };

            User? user = await context
                .User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            bool IsTokenUsed = user.UserTokens?.FirstOrDefault(
                x => x.UserIpAddress == emailModel.UserIp) is not null;
            
            if (IsTokenUsed) return Forbid("Invalid token used");

            UserAdditionalInfo userInfo = user.UserAdditionalInfo!;

            if (userInfo.IsConfirmedAccount is false)
            {
                userInfo.IsConfirmedAccount = true;

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

            await context.UserToken.AddAsync(newUserToken);
            await context.SaveChangesAsync();

            return Ok(tokenModel);
        }

        [Authorize]
        [HttpDelete("{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserToken? userToken =  await context.UserToken
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == UserId && x.RefreshToken == refreshToken);

            if (userToken == null) return Forbid("Invalid token");

            context.UserToken.Remove(userToken!);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("all/{refreshToken}")]
        public async Task<IActionResult> LogoutAll(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserToken> userTokens = await context.UserToken
                .AsNoTracking()
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            if (userTokens.Count == 0) return Forbid("Invalid token");

            context.UserToken.RemoveRange(userTokens);
            await context.SaveChangesAsync();

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
