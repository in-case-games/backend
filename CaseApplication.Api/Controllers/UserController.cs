using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository= userRepository;
        }

        [HttpGet]
        public async Task<User> GetUser(string email, string hash)
        {
            return await _userRepository.GetUser(email);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            List<User> users = (await _userRepository.GetAllUsers()).ToList();
            for(int i = 0; i < users.Count; i++)
            {
                users[i].PasswordSalt = "";
            }
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var temp = new User()
            {
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserImage = user.UserImage,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
            };
            return Ok(await _userRepository.CreateUser(temp));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id, UserDto userRemoving)
        {
            return Ok(await _userRepository.DeleteUser(id));
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto user, string hash)
        {
            var temp = new User()
            {
                Id = user.Id,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserImage = user.UserImage,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
            return Ok(await _userRepository.UpdateUser(temp));
        }
    }
}
