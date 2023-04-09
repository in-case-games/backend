using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Authentication.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserDto userDto)
        {
            //Check is exist user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                x.Id == userDto.Id ||
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (user is null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!))
                return Forbid("Invalid data");

            return await _emailService.SendSignIn(new DataMailLink()
            {
                UserEmail = user.Email!,
                UserLogin = user.Login!,
                EmailToken = _jwtService.CreateEmailToken(user),
                UserIp = userDto.Ip!,
                UserPlatforms = userDto.Platform!,
            });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExist = await context.Users
                .AsNoTracking()
                .AnyAsync(x =>
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (isExist) 
                return ResponseUtil.Conflict("User already exists!");

            //Map user and additional info
            User user = userDto.Convert();

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(userDto.Password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == "user");

            UserAdditionalInfo info = new() {
                RoleId = role!.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
            };

            try
            {
                await _emailService.SendSignUp(new DataMailLink()
                {
                    UserEmail = user.Email!,
                    UserLogin = user.Login!,
                    EmailToken = _jwtService.CreateEmailToken(user)
                });
            }
            catch (SmtpCommandException)
            {
                return ResponseUtil.Conflict("MailBox is not existed!");
            }

            //Create user and additional info
            await context.Users.AddAsync(user);
            await context.UserAdditionalInfos.AddAsync(info);

            await context.SaveChangesAsync();

            return ResponseUtil.SendEmail();
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            ClaimsPrincipal? principal = _jwtService.GetClaimsToken(refreshToken); 

            if(principal is null) 
                return Forbid("Invalid refresh token");

            string id = principal.Claims
                .Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value;

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user is null) 
                return ResponseUtil.NotFound("User");
            if (!ValidationService.IsValidToken(in user, principal, "refresh"))
                return Forbid("Invalid refresh token");
            
            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user!);

            return ResponseUtil.Ok(tokenModel);
        }
    }
}
