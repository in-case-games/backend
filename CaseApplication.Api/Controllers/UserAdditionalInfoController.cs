using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private readonly IUserRepository _userRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(
            IUserAdditionalInfoRepository userInfoRepository,
            IUserRepository userRepository)
        {
            _userInfoRepository = userInfoRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.GetByUserId(UserId);
            if (userAdditionalInfo != null) {
                return Ok(userAdditionalInfo);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            User? searchUser = await _userRepository.GetByParameters(user);

            if (searchUser == null) return NotFound();

            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.Get(searchUser.Id);

            if(userAdditionalInfo != null) return BadRequest();
            
            await _userInfoRepository.Create(new() { 
                UserId = searchUser!.Id 
            });
            
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateInfoByAdmin(UserAdditionalInfo userAdditionalInfo)
        {
            return Ok(await _userInfoRepository.Update(userAdditionalInfo));
        }
    }
}
