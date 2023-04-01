using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly ValidationService _validationService;
        #endregion
        #region ctor
        public AuthenticationController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            JwtService jwtService,
            EmailService emailService,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _jwtService = jwtService;
            _emailService = emailService;
            _validationService = validationService;
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
                return NotFound(new { Success = false, Message = "User not found the update is not available" });

            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!))
                return Forbid("Access is denied incorrectly entered data");

            await _emailService.SendSignIn(new DataMailLink()
            {
                UserEmail = user.Email!,
                UserLogin = user.Login!,
                EmailToken = _jwtService.CreateEmailToken(user),
                UserIp = userDto.Ip!,
                UserPlatforms = userDto.Platform!,
            });

            return Ok(new
            {
                Success = true,
                Message = "Authentication success. Check your email for the following actions"
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
                return Conflict(new { Success = false, Message = "User already exists!" });

            //Map user and additional info
            User user = userDto.Convert();

            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            user.PasswordHash = EncryptorService.GenerationHashSHA512(userDto.Password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == "user") ?? throw new Exception("Eblan dobavb roli");

            UserAdditionalInfo info = new() {
                RoleId = role.Id,
                UserId = user.Id,
                DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30),
            };

            //Create user and additional info
            await context.Users.AddAsync(user);
            await context.UserAdditionalInfos.AddAsync(info);

            await context.SaveChangesAsync();

            await _emailService.SendSignUp(new DataMailLink()
            {
                UserEmail = user.Email!,
                UserLogin = user.Login!,
                EmailToken = _jwtService.CreateEmailToken(user)
            });

            return Ok(new
            {
                Success = true,
                Message = "Registation success. Check your email for the following actions"
            });
        }

        [AllowAnonymous]
        [HttpGet("refresh/{login}")]
        public async Task<IActionResult> RefreshTokens(string login, string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login);

            if (user is null) 
                return NotFound(new { Success = false, Message = "User not found the update is not available" });

            string secret = user.PasswordHash + user.Email;

            if(!_validationService.IsValidToken(refreshToken, secret))
                return Forbid("Invalid refresh token");
            
            DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user!);

            return Ok(new 
            { 
                Success = true, 
                Data = tokenModel 
            });
        }
    }
}
