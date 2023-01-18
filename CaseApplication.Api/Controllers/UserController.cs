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
        public async Task<IActionResult> CreateUser(User user)
        {
            return Ok(await _userRepository.CreateUser(user));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id, UserDto userRemoving)
        {
            return Ok(await _userRepository.DeleteUser(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user, string hash)
        {
            return Ok(await _userRepository.UpdateUser(user));
        }
    }
}
