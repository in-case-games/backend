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
        [HttpPost("user/confirm")]
        public async Task<IActionResult> SendConfirmEmail(EmailModel emailModel)
        {
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid("Incorrect email");

            emailModel.UserToken = _jwtHelper.GenerateEmailToken(user, emailModel.UserIp);

            await _emailHelper.SendConfirmAccountToEmail(emailModel);

            return Accepted();
        }

        [Authorize]
        [HttpPut("user/email/{password}")]
        public async Task<IActionResult> SendUpdateEmail(EmailModel emailModel, string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (_validationService.IsValidEmailTokenSend(in user, emailModel.UserIp, password))
            {
                MapEmailModelForSend(ref emailModel, user);
                await _emailHelper.SendChangeEmailToEmail(emailModel);

                return Accepted();
            }

            //TODO Give temp password

            return Forbid("Incorrect password/ip");
        }
        
        [Authorize]
        [HttpPut("user/password/{password}")]
        public async Task<IActionResult> SendUpdatePassword(EmailModel emailModel, string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (_validationService.IsValidEmailTokenSend(in user, emailModel.UserIp, password))
            {
                MapEmailModelForSend(ref emailModel, user);
                await _emailHelper.SendChangePasswordToEmail(emailModel);

                return Accepted();
            }

            //TODO Give temp password

            return Forbid("Incorrect password/ip");
        }

        [Authorize]
        [HttpDelete("user/{password}")]
        public async Task<IActionResult> SendDeleteAccount(EmailModel emailModel, string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();
            if (_validationService.IsValidEmailTokenSend(in user, emailModel.UserIp, password))
            {
                MapEmailModelForSend(ref emailModel, user);
                await _emailHelper.SendDeleteAccountToEmail(emailModel);

                return Accepted();
            }

            //TODO Give temp password

            return Forbid("Incorrect password/ip");
        }

        private void MapEmailModelForSend(ref EmailModel emailModel, User user)
        {
            emailModel.UserEmail = user.UserEmail!;
            emailModel.UserId = user.Id;
            emailModel.UserToken = _jwtHelper.GenerateEmailToken(user, emailModel.UserIp);
        }
    }
}
