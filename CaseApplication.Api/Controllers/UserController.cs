using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
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
        public async Task<User> Get(string email, string hash)
        {
            return await _userRepository.Get(email);
        }

        [HttpGet("GetByLogin")]
        public async Task<User> GetUserByLogin(string login, string hash)
        {
            return await _userRepository.GetByLogin(login);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAll()
        {
            List<User> users = (await _userRepository.GetAll()).ToList();
            for(int i = 0; i < users.Count; i++)
            {
                users[i].PasswordSalt = "";
            }
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            return Ok(await _userRepository.Create(user));
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user, string hash)
        {
            return Ok(await _userRepository.Update(user));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userRepository.Delete(id));
        }
    }
}
