using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CaseApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserTokensRepository _userTokensRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IUserRepository userRepository, 
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            IUserTokensRepository userTokensRepository)
        {
            _userRepository = userRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _userTokensRepository = userTokensRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(Guid? userId = null)
        {
            User? user = await _userRepository.Get(userId ?? UserId);

            if(user != null)
            {
                user.PasswordHash = "";
                user.PasswordSalt = "";

                return Ok(user);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("GetByLogin")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            User? user = await _userRepository.GetByLogin(login);

            if (user != null)
            {
                user.PasswordHash = "";
                user.PasswordSalt = "";

                return Ok(user);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPut("UpdateLogin")]
        public async Task<IActionResult> Update(string login)
        {
            User? searchUserByLogin = await _userRepository.GetByLogin(login);
            User? searchUserById = await _userRepository.Get(UserId);

            if (searchUserByLogin != null) return BadRequest();
            if (searchUserById == null) return NotFound();

            User newUser = new() { 
                Id = searchUserById.Id,
                PasswordHash = searchUserById.PasswordHash,
                PasswordSalt = searchUserById.PasswordSalt,
                UserEmail = searchUserById.UserEmail,
                UserImage = searchUserById.UserImage,
                UserLogin = login
            };

            return Ok(await _userRepository.Update(searchUserById, newUser));
        }

        [Authorize]
        [HttpPost("SendDeleteAccount")]
        public async Task<IActionResult> SendDeleteAccount(string password)
        {
            User? user = await _userRepository.Get(UserId);

            if (user == null) return NotFound();

            string genHash = _encryptorHelper.EncryptorPassword(password, Convert
                .FromBase64String(user.PasswordSalt!));

            if (genHash != user.PasswordHash) return BadRequest("Incorrect password");



            return BadRequest();
        }

        [AllowAnonymous]
        [HttpDelete("DeleteConfirmation")]
        public async Task<IActionResult> DeleteConfirmation(Guid userId, string oneTimeToken)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsOneTimeToken(oneTimeToken, user.PasswordHash!);

            if (principal is null)
                return BadRequest("Invalid OneTime token");

            //TODO Answer user by email
            //TODO No delete give the user 30 days

            return Ok(await _userRepository.Delete(userId));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteByAdmin")]
        public async Task<IActionResult> DeleteByAdmin(Guid userId)
        {
            User? user = await _userRepository.Get(userId);

            if (user != null) {
                return Ok(await _userRepository.Delete(userId));
            };

            return NotFound();
        }
    }
}
