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
    public class EmailController : ControllerBase
    {
        private readonly EmailHelper _emailHelper;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokensRepository _userTokensRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public EmailController(
            EmailHelper emailHelper,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            IUserRepository userRepository, 
            IUserTokensRepository userTokensRepository)
        {
            _emailHelper = emailHelper;
            _userRepository = userRepository;
            _userTokensRepository = userTokensRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("user/confirm/{userId}&{email}")]
        public async Task<IActionResult> SendConfirmEmail(Guid userId, string email)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();
            if (user.UserEmail != email) return Forbid("Incorrect email");

            string oneTimeToken = GenerateOneTimeToken(user.PasswordHash!);

            await _emailHelper.SendConfirmAccountToEmail(
                user.UserEmail!,
                user.Id.ToString(),
                oneTimeToken);

            return Accepted();
        }

        [AllowAnonymous]
        [HttpPost("user/activate/{userId}&{email}")]
        public async Task<IActionResult> SendActivateEmail(Guid userId, string email)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();
            if (user.UserEmail != email) return Forbid("Incorrect email");

            string oneTimeToken = GenerateOneTimeToken(user.PasswordHash!);

            await _emailHelper.SendActivateAccountToEmail(
                user.UserEmail!,
                user.Id.ToString(),
                oneTimeToken);

            return Accepted();
        }

        [Authorize]
        [HttpPut("user/email/{password}")]
        public async Task<IActionResult> SendUpdateEmail(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (IsValidPassword(in user, password) is false) return Forbid("Incorrect password");

            string oneTimeToken = GenerateOneTimeToken(user.PasswordHash!);

            await _emailHelper.SendChangeEmailToEmail(
                user.UserEmail!,
                user.Id.ToString(),
                oneTimeToken);

            return Accepted();
        }
        
        [Authorize]
        [HttpPut("user/password/{password}")]
        public async Task<IActionResult> SendUpdatePassword(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (IsValidPassword(in user, password) is false) return Forbid("Incorrect password");

            string oneTimeToken = GenerateOneTimeToken(user.PasswordHash!);

            await _emailHelper.SendChangePasswordToEmail(
                user.UserEmail!, 
                user.Id.ToString(), 
                oneTimeToken);

            return Accepted();
        }

        [Authorize]
        [HttpDelete("user/{password}")]
        public async Task<IActionResult> SendDeleteAccount(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (IsValidPassword(in user, password) is false) return Forbid("Incorrect password");

            string oneTimeToken = GenerateOneTimeToken(user.PasswordHash!);

            await _emailHelper.SendDeleteAccountToEmail(
                user.UserEmail!, 
                user.Id.ToString(), 
                oneTimeToken);

            return Accepted();
        }

        private bool IsValidPassword(in User user, string password)
        {
            string hash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        private string GenerateOneTimeToken(string hash)
        {
            Claim[] claims = {
                new Claim("UserId", UserId.ToString())
            };

            JwtSecurityToken token = _jwtHelper.CreateOneTimeToken(claims, hash);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
