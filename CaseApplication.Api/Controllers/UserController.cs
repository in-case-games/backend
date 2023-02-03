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

        //TODO Delete admin for user

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(string refreshToken, string ip)
        {
            ClaimsPrincipal? principal = _jwtHelper.GetPrincipalFromExpiredToken(refreshToken);

            if (principal is null)
                return BadRequest("Invalid refresh token");

            UserToken? userToken = await _userTokensRepository.GetByIp(UserId, ip);

            if (userToken == null ||
                refreshToken != userToken.RefreshToken ||
                userToken.RefreshTokenExpiryTime <= DateTime.Now)
            {
                await _userTokensRepository.DeleteByToken(UserId, refreshToken);
                //TODO Answer user by email HACK!
                return BadRequest("Invalid refresh token");
            }

            return Ok(await _userRepository.Delete(UserId));
        }
    }
}
