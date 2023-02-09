using AutoMapper;
using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
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
                user.PasswordHash = "access denied";
                user.PasswordSalt = "access denied";

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
                user.PasswordHash = "access denied";
                user.PasswordSalt = "access denied";
                user.UserTokens = null;

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
                user.UserEmail = "access denied";
                user.PasswordHash = "access denied";
                user.PasswordSalt = "access denied";
            }

            return Ok(users);
        }

        [Authorize]
        [HttpPut("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login)
        {
            User? searchUserByLogin = await _userRepository.GetByLogin(login);
            User? searchUserById = await _userRepository.Get(UserId);

            if (searchUserByLogin != null) return BadRequest();
            if (searchUserById == null) return NotFound();

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            UserDto user = mapper.Map<UserDto>(searchUserById);
            UserDto newUser = mapper.Map<UserDto>(searchUserById);
            newUser.UserLogin = login;

            await _userRepository.Update(user, newUser);

            await _emailHelper.SendNotifyToEmail(
                searchUserById.UserEmail!, 
                "Администрация сайта" , 
                new EmailPatternModel()
                {
                    Body = $"Имя вашего акканута измененно на: {login}"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(EmailModel emailModel)
        {
            User? searchUser = await _userRepository.GetByEmail(emailModel.UserEmail);
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");
            if (searchUser != null) return Forbid("Email is already busy");

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            UserDto oldUser = mapper.Map<UserDto>(user);
            UserDto newUser = mapper.Map<UserDto>(user);

            newUser.UserEmail = emailModel.UserEmail;
            user.UserAdditionalInfo!.IsConfirmedAccount = false;

            await _userRepository.Update(oldUser, newUser);
            await _userInfoRepository.Update(user.UserAdditionalInfo);

            await _userTokensRepository.DeleteAll(user.Id);

            await _emailHelper.SendNotifyToEmail(
                emailModel.UserEmail,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вы изменили email аккаунта"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePasswordConfirmation(EmailModel emailModel, string password)
        {
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            UserDto oldUser = mapper.Map<UserDto>(user);
            UserDto newUser = mapper.Map<UserDto>(user);
            newUser.PasswordHash = hash;
            newUser.PasswordSalt = Convert.ToBase64String(salt);

            await _userRepository.Update(oldUser, newUser);
            await _userTokensRepository.DeleteAll(user.Id);

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вы сменили пароль"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmation(EmailModel emailModel)
        {
            User? user = await _userRepository.Get(emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Ваш аккаунт будет удален через 30 дней"
                });

            //TODO No delete give the user 30 days

            await _userTokensRepository.DeleteAll(emailModel.UserId);

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
