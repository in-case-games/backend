using InCase.Domain.Dtos;
using InCase.Domain.Entities.Auth;
using InCase.Domain.Entities.Email;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Authentication.Api.Controllers
{
    [Route("auth/api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region injections
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
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
        public async Task<IActionResult> SignIn(
            UserDto userDto,
            string ip = "",
            string platform = "")
        {
            //Check is exist user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .Include(x => x.Inventories)
                .Include(x => x.HistoryPromocodes)
                .Include(x => x.Restrictions)
                .Include(x => x.HistoryOpenings)
                .FirstOrDefaultAsync(x =>
                x.Id == userDto.Id ||
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (user is null) return NotFound();
            if (!ValidationService.IsValidUserPassword(in user, userDto.Password!)) return Forbid();

            await _emailService.SendSignIn(new DataMailLink()
            {
                UserEmail = user.Email!,
                UserName = user.Login!,
                EmailToken = _jwtService.CreateEmailToken(user),
                UserIp = ip,
                UserPlatforms = platform,
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
            //Check is exist user
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? userExists = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.Email == userDto.Email ||
                x.Login == userDto.Login);

            if (userExists is not null) return Conflict(new { Success = false, Message = "User already exists!" });

            //Encrypting password
            byte[] salt = EncryptorService.GenerationSaltTo64Bytes();

            User user = userDto.Convert();
            user.Id = Guid.NewGuid();
            user.PasswordHash = EncryptorService.GenerationHashSHA512(userDto.Password!, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);

            await context.Users.AddAsync(user);

            //Create Add info
            User? createdUser = await context.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Login == user.Login);

            UserAdditionalInfo info = new();

            UserRole? role = await context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == "user");

            info.Id = Guid.NewGuid();
            info.RoleId = role!.Id;
            info.UserId = user.Id!;

            await context.UserAdditionalInfos.AddAsync(info);
            await context.SaveChangesAsync();

            await _emailService.SendSignUp(new DataMailLink()
            {
                UserEmail = user.Email!,
                UserName = user.Login!,
                EmailToken = _jwtService.CreateEmailToken(user)
            });

            return Ok(new
            {
                Success = true,
                Message = "Registation success. Check your email for the following actions"
            });
        }

        [AllowAnonymous]
        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> RefreshTokens(string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            //Get User id
            string? getUserId = _jwtService.GetIdFromRefreshToken(refreshToken);

            if (getUserId == null) return Forbid("Invalid refresh token");

            _ = Guid.TryParse(getUserId, out Guid userId);

            //Search refresh token by ip TODO Cut in method
            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .Include(x => x.AdditionalInfo!.Role)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (_validationService.IsValidToken(refreshToken, in user!))
            {
                //Send token
                DataSendTokens tokenModel = _jwtService.CreateTokenPair(in user!);

                return Ok(new { Data = tokenModel, Success = true });
            }

            return NoContent();
        }
    }
}
