using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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

        public UserController(
            IUserRepository userRepository, 
            EncryptorHelper encryptorHelper)
        {
            _userRepository = userRepository;
            _encryptorHelper = encryptorHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id, string hash)
        {
            User? user = await _userRepository.Get(id);

            if(user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail(string email, string hash)
        {
            User? user = await _userRepository.GetByEmail(email);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet("GetByLogin")]
        public async Task<IActionResult> GetByLogin(string login, string hash)
        {
            User? user = await _userRepository.GetByLogin(login);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<User> users = await _userRepository.GetAll();
            foreach (User t in users)
            {
                t.PasswordSalt = "";
                t.PasswordHash = "";
            }
            return Ok(users);
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
