using AutoMapper;
using CaseApplication.Domain.Dtos;
using CaseApplication.Domain.Entities.Auth;
using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("auth/api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
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
            JwtService jwtService,
            EmailService emailService)
        {
            _contextFactory = contextFactory;
            _jwtService = jwtService;
            _emailService = emailService;
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
            if (!ValidationService.IsValidUserPassword(in user, password)) return Forbid();

            //Check exceeded sessions
            bool isExceededSessions = user.UserTokens?.Count >= 100;

            if (isExceededSessions)
            {
                await _emailService.SendNotifyToEmail(
                    user.UserEmail!,
                    "Администрация сайта",
                    new EmailTemplate()
                    {
                        BodyDescription = $"Превышенно количество сессий для входа в аккаунт. " +
                        $"Чтобы войти под новым устройством, выйдите с прошлого"
                    });

                return Forbid("Exceeded the number of sessions");
            }

            await _emailService.SendSignIn(new DataMailLink()
            {
                UserEmail = user.UserEmail!,
                UserId = user.Id,
                EmailToken = _jwtService.GenerateEmailToken(user),
                UserIp = ip,
                UserPlatforms = platform,
            }, user.UserLogin!);

            return Ok(new { Success = true,
                Message = "Authentication success. Check your email for the following actions" });
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

            if (userExists is not null) return Conflict(new { Success = false, Message = "User already exists!" });

            //Encrypting password
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            userDto.PasswordHash = EncryptorService.GenerationHashSHA512(password, salt);
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

            await _emailService.SendSignUp(new DataMailLink()
            {
                UserEmail = user.UserEmail!,
                UserId = user.Id,
                EmailToken = _jwtService.GenerateEmailToken(user)
            }, user.UserLogin!);

            return Ok(new { Success = true,
                Message = "Registation success. Check your email for the following actions" });
        }

        [AllowAnonymous]
        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //Get User id
            string? getUserId = _jwtService.GetIdFromRefreshToken(refreshToken);

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
                await _emailService.SendSignIn(new DataMailLink()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    EmailToken = _jwtService.GenerateEmailToken(user),
                }, user.UserLogin!);

                context.UserToken.Remove(userToken);

                return NoContent();
            }

            //Update and send token
            DataSendTokens tokenModel = _jwtService.GenerateTokenPair(in user);

            MapUserTokenForUpdate(ref userToken!, tokenModel);

            await context.SaveChangesAsync();

            return Ok(new { Data = tokenModel, Success = true });
        }

        [Authorize]
        [HttpDelete("logout/{refreshToken}")]
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
        [HttpDelete("logout/all")]
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

        private static void MapUserTokenForUpdate(ref UserToken userToken, DataSendTokens tokenModel)
        {
            userToken.RefreshToken = tokenModel.RefreshToken;
            userToken.RefreshTokenExpiryTime = tokenModel.ExpiresRefreshIn;
            userToken.RefreshTokenCreationTime = DateTime.UtcNow;
        }
    }
}
