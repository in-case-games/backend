using AutoMapper;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private readonly ValidationService _validationService;
        private MapperConfiguration mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<User, UserDto>();
        }
        );
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IUserRepository userRepository, 
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            EmailHelper emailHelper,
            IUserTokensRepository userTokensRepository,
            IUserAdditionalInfoRepository userAdditionalInfoRepository,
            ValidationService validationService)
        {
            _userRepository = userRepository;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
            _userTokensRepository = userTokensRepository;
            _userInfoRepository = userAdditionalInfoRepository;
            _validationService = validationService;
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
        [HttpGet("login/{login}")]
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
        [HttpGet("all")]
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

        [Authorize]
        [HttpPut("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login)
        {
            User? searchUserByLogin = await _userRepository.GetByLogin(login);
            User? searchUserById = await _userRepository.Get(UserId);

            IMapper? mapper = mapperConfiguration.CreateMapper();
            UserDto user = mapper.Map<UserDto>(searchUserById);

            if (searchUserByLogin != null) return BadRequest();
            if (searchUserById == null) return NotFound();

            UserDto newUser = new() { 
                Id = searchUserById.Id,
                PasswordHash = searchUserById.PasswordHash,
                PasswordSalt = searchUserById.PasswordSalt,
                UserEmail = searchUserById.UserEmail,
                UserImage = searchUserById.UserImage,
                UserLogin = login
            };

            await _userRepository.Update(user, newUser);

            await _emailHelper.SendNotifyChangeLogin(searchUserById.UserEmail!, login);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("email/{userId}&{email}&{token}")]
        public async Task<IActionResult> UpdateEmail(Guid userId, string email, string token)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();

            if (_validationService.IsValidEmailToken(token, user.PasswordHash!) is false)
                return Forbid("Invalid email token");

            User? searchUser = await _userRepository.GetByEmail(email);

            if(searchUser != null) return Forbid("Email is already busy");

            IMapper? mapper = mapperConfiguration.CreateMapper();
            UserDto oldUser = mapper.Map<UserDto>(user);

            UserDto newUser = new()
            {
                Id = user.Id,
                UserEmail = email,
                UserImage = user.UserImage,
                UserLogin = user.UserLogin,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
            };

            await _userRepository.Update(oldUser, newUser);

            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.GetByUserId(user.Id);
            userAdditionalInfo!.IsConfirmedAccount = false;

            await _userInfoRepository.Update(userAdditionalInfo);
            await _userTokensRepository.DeleteAll(userId);

            await _emailHelper.SendNotifyChangeEmail(email);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("password/{userId}&{password}&{token}")]
        public async Task<IActionResult> UpdatePasswordConfirmation(
            Guid userId, 
            string password, 
            string token)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();

            if (_validationService.IsValidEmailToken(token, user.PasswordHash!) is false)
                return Forbid("Invalid email token");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            IMapper? mapper = mapperConfiguration.CreateMapper();
            UserDto oldUser = mapper.Map<UserDto>(user);

            UserDto newUser = new()
            {
                Id = user.Id,
                UserEmail = user.UserEmail,
                UserImage = user.UserImage,
                UserLogin = user.UserLogin,
                PasswordHash = hash,
                PasswordSalt = Convert.ToBase64String(salt)
            };

            await _userRepository.Update(oldUser, newUser);
            await _userTokensRepository.DeleteAll(userId);

            await _emailHelper.SendNotifyChangePassword(user.UserEmail!);

            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("{userId}&{token}")]
        public async Task<IActionResult> DeleteConfirmation(Guid userId, string token)
        {
            User? user = await _userRepository.Get(userId);

            if (user == null) return NotFound();

            if (_validationService.IsValidEmailToken(token, user.PasswordHash!) is false)
                return Forbid("Invalid email token");

            await _emailHelper.SendNotifyDeleteAccount(user.UserEmail!);
            //TODO No delete give the user 30 days

            await _userTokensRepository.DeleteAll(userId);

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{userId}")]
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
