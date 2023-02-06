﻿using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly EmailHelper _emailHelper;
        private readonly IUserTokensRepository _userTokensRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IUserRepository userRepository, 
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            EmailHelper emailHelper,
            IUserTokensRepository userTokensRepository)
        {
            _userRepository = userRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
            _userTokensRepository = userTokensRepository;
        }

        [Authorize]
        [HttpGet("{userId}")]
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
        [HttpGet("{login}")]
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<User> users = await _userRepository.GetAll();

            foreach(User user in users)
            {
                user.PasswordHash = "";
                user.PasswordSalt = "";
            }

            return Ok(users);
        }

        //TODO Change Email
        //TODO Change Password

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateLogin(string login)
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
        [HttpPost]
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

        [AllowAnonymous]
        [HttpDelete("{userId}&{token}")]
        public async Task<IActionResult> DeleteConfirmation(Guid userId, string token)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();

            byte[] secretBytes = Encoding.UTF8.GetBytes(user.PasswordHash!);

            ClaimsPrincipal? principal = _jwtHelper
                .GetClaimsToken(token, secretBytes, "HS512");

            if (principal is null)
                return BadRequest("Invalid OneTime token");

            //TODO Answer user by email
            //TODO No delete give the user 30 days
            
            await _userTokensRepository.DeleteAll(UserId);

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteByAdmin(Guid userId)
        {
            User? searchUser = await _userRepository.Get(userId);

            if (searchUser != null) {
                return Ok(await _userRepository.Delete(userId));
            };

            return NotFound();
        }
    }
}
