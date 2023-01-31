using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CaseApplication.Api.Controllers
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
        public async Task<User> Get(Guid id, string hash)
        {
            return await _userRepository.Get(id);
        }

        [HttpGet("GetByEmail")]
        public async Task<User> GetByEmail(string email, string hash)
        {
            return await _userRepository.GetByEmail(email);
        }

        [HttpGet("GetByLogin")]
        public async Task<User> GetByLogin(string login, string hash)
        {
            return await _userRepository.GetByLogin(login);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAll()
        {
            List<User> users = (await _userRepository.GetAll()).ToList();
            foreach (User t in users)
            {
                t.PasswordSalt = "";
                t.PasswordHash = "";
            }
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string password)
        {
            byte[] salt;
            string saltEncoding;
            do
            {
                salt = RandomNumberGenerator.GetBytes(256 / 8);
                saltEncoding = Convert.ToBase64String(salt);
            }
            while (!await _userRepository.IsUniqueSalt(saltEncoding));

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));

            user.PasswordHash = hashed;
            user.PasswordSalt = saltEncoding;

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
