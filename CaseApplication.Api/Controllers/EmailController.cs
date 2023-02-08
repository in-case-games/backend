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
    public class EmailController : ControllerBase
    {
        private readonly EmailHelper _emailHelper;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokensRepository _userTokensRepository;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public EmailController(
            EmailHelper emailHelper,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            IUserRepository userRepository, 
            IUserTokensRepository userTokensRepository,
            ValidationService validationService)
        {
            _emailHelper = emailHelper;
            _userRepository = userRepository;
            _userTokensRepository = userTokensRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _validationService = validationService;
        }

        [AllowAnonymous]
        [HttpPost("user/confirm/{userId}&{email}&{ip}")]
        public async Task<IActionResult> SendConfirmEmail(Guid userId, string email, string ip)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();
            if (user.UserEmail != email) return Forbid("Incorrect email");

            string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

            await _emailHelper.SendConfirmAccountToEmail(new EmailModel()
            {
                UserEmail = user.UserEmail!,
                UserId = user.Id,
                UserToken = emailToken,
                UserIp = ip
            });

            return Accepted();
        }

        [AllowAnonymous]
        [HttpPost("user/activate/{userId}&{email}&{ip}")]
        public async Task<IActionResult> SendActivateEmail(Guid userId, string email, string ip)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();
            if (user.UserEmail != email) return Forbid("Incorrect email");

            string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

            await _emailHelper.SendActivateAccountToEmail(new EmailModel()
            {
                UserEmail = user.UserEmail!,
                UserId = user.Id,
                UserToken = emailToken,
                UserIp = ip
            });

            return Accepted();
        }

        [Authorize]
        [HttpPut("user/email/{password}")]
        public async Task<IActionResult> SendUpdateEmail(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();

            if (_validationService.IsValidUserPassword(in user, password)) {
                string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

                await _emailHelper.SendChangeEmailToEmail(new EmailModel()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    UserToken = emailToken,
                });

                return Accepted();
            }

            return Forbid("Incorrect password");
        }
        
        [Authorize]
        [HttpPut("user/password/{password}")]
        public async Task<IActionResult> SendUpdatePassword(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();

            if (_validationService.IsValidUserPassword(in user, password))
            {
                string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

                await _emailHelper.SendChangePasswordToEmail(new EmailModel()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    UserToken = emailToken,
                });

                return Accepted();
            }

            return Forbid("Incorrect password");
        }

        [Authorize]
        [HttpDelete("user/{password}")]
        public async Task<IActionResult> SendDeleteAccount(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();

            if (_validationService.IsValidUserPassword(in user, password))
            {
                string emailToken = _jwtHelper.GenerateEmailToken(user.Id, user.PasswordHash!);

                await _emailHelper.SendDeleteAccountToEmail(new EmailModel()
                {
                    UserEmail = user.UserEmail!,
                    UserId = user.Id,
                    UserToken = emailToken,
                });

                return Accepted();
            }

            return Forbid("Incorrect password");
        }
    }
}
