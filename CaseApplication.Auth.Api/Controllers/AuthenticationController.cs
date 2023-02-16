﻿using AutoMapper;
using CaseApplication.Domain.Dtos;
using CaseApplication.Domain.Entities;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using CaseApplication.Infrastructure.Services;
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
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
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
        [HttpPost("signin/{password}")]
        public async Task<IActionResult> SignIn(
            UserDto userDto, 
            string password, 
            string ip = "", 
            string platform = "")
        {
            //Check is exist user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .Include(x => x.UserTokens)
                .FirstOrDefaultAsync(x =>
                x.Id == userDto.Id ||
                x.UserEmail == userDto.UserEmail ||
                x.UserLogin == userDto.UserLogin);

            if (user is null) return NotFound();
            if (!_validationService.IsValidUserPassword(in user, password)) return Forbid();

            //Check exceeded sessions
            bool isExceededSessions = user.UserTokens?.Count >= 100;

            if (isExceededSessions)
            {
                await _emailHelper.SendNotifyToEmail(
                    user.UserEmail!,
                    "Администрация сайта",
                    new EmailPatternModel()
                    {
                        Body = $"Превышенно количество сессий для входа в аккаунт. " +
                        $"Чтобы войти под новым устройством, выйдите с прошлого"
                    });

                return Forbid("Exceeded the number of sessions");
            }

            await _emailHelper.SendConfirmAccountToEmail(new EmailModel()
            {
                UserEmail = user.UserEmail!,
                UserId = user.Id,
                EmailToken = _jwtHelper.GenerateEmailToken(user),
                UserIp = ip,
                UserPlatforms = platform,
            });

            return Unauthorized("Check email");
        }

        [AllowAnonymous]
        [HttpPost("signup/{password}")]
        public async Task<IActionResult> SignUp(UserDto userDto, string password)
        {
            //Check is exist user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? userExists = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.UserLogin == userDto.UserLogin);

            if (userExists is not null) return Conflict();

            //Encrypting password
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();

            userDto.PasswordHash = _encryptorHelper.EncryptorPassword(password, salt);
            userDto.PasswordSalt = Convert.ToBase64String(salt);

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            User user = mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);

            //Create Add info
            User? createdUser = await context.User
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.UserLogin == user.UserLogin);

            UserAdditionalInfo info = new();

            UserRole? role = await context.UserRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleName == "user");

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
        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //Get User id
            string? getUserId = _jwtHelper.GetIdFromRefreshToken(refreshToken);

            if(getUserId == null) return Forbid("Invalid refresh token");

            _ = Guid.TryParse(getUserId, out Guid userId);

            //Search refresh token by ip TODO Cut in method
            User? user = await context.User
                .Include(x => x.UserTokens)
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .FirstOrDefaultAsync(x => x.Id == userId);

            UserToken? userToken = user!.UserTokens!.FirstOrDefault(x => x.RefreshToken == refreshToken);

            if(userToken == null) return Forbid("Invalid refresh token");

            if (userToken.RefreshTokenExpiryTime <= DateTime.UtcNow) {
                await _emailHelper.SendNotifyToEmail(
                    user.UserEmail!,
                    "Администрация сайта",
                    new EmailPatternModel()
                    {
                        Body = $"Попытка входа в аккаунт"
                    });

                context.UserToken.Remove(userToken);

                return NoContent();
            }

            //Update and send token
            TokenModel tokenModel = _jwtHelper.GenerateTokenPair(in user);

            MapUserTokenForUpdate(ref userToken!, tokenModel);

            await context.SaveChangesAsync();

            return Ok(tokenModel);
        }

        //TODO
        [AllowAnonymous]
        [HttpGet("confirm/{userId}&{token}")]
        public async Task<IActionResult> ConfirmAccount(
            Guid userId, 
            string token,
            string ip = "",
            string platform = "")
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //TODO OneTimeToken
            EmailModel emailModel = new()
            {
                UserId = userId,
                EmailToken = token,
                UserIp = ip,
                UserPlatforms = platform
            };

            User? user = await context.User
                .Include(x => x.UserTokens)
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(in emailModel, in user);

            if (isValidToken is false) return Forbid("Invalid email token");

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
                Id = new Guid(),
                UserId = user.Id,
                UserIpAddress = emailModel.UserIp,
                UserPlatfrom = emailModel.UserPlatforms,
                EmailToken = emailModel.EmailToken,
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

            UserToken? userToken = await context.UserToken
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == UserId && x.RefreshToken == refreshToken);

            if (userToken == null) return Forbid("Invalid token");

            context.UserToken.Remove(userToken);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("all")]
        public async Task<IActionResult> LogoutAll()
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