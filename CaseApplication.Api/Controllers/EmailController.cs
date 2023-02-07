using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpPut("user/email/{email}")]
        public async Task<IActionResult> SendUpdateEmail(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();

            string genHash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            if (genHash != user.PasswordHash) return Forbid("Incorrect password");

            Claim[] claims = {
                new Claim("UserId", user.Id.ToString())
            };

            JwtSecurityToken token = _jwtHelper.CreateOneTimeToken(claims, genHash);
            string oneTimeToken = new JwtSecurityTokenHandler().WriteToken(token);

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

            string genHash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            if (genHash != user.PasswordHash) return Forbid("Incorrect password");

            Claim[] claims = {
                new Claim("UserId", user.Id.ToString())
            };

            JwtSecurityToken token = _jwtHelper.CreateOneTimeToken(claims, genHash);
            string oneTimeToken = new JwtSecurityTokenHandler().WriteToken(token);

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

            string genHash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            if (genHash != user.PasswordHash) return Forbid("Incorrect password");

            Claim[] claims = {
                new Claim("UserId", user.Id.ToString())
            };

            JwtSecurityToken token = _jwtHelper.CreateOneTimeToken(claims, genHash);
            string oneTimeToken = new JwtSecurityTokenHandler().WriteToken(token);

            await _emailHelper.SendDeleteAccountToEmail(user.UserEmail!, user.Id.ToString(), oneTimeToken);

            return Accepted();
        }
    }
}
