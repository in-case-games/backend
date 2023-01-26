using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IUserAdditionalInfoRepository _userInfoRepository;

        public UserAdditionalInfoController(IUserAdditionalInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [HttpGet]
        public async Task<UserAdditionalInfo> Get(Guid userId)
        {
            return await _userInfoRepository.Get(userId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserAdditionalInfo userInfo)
        {
            return Ok(await _userInfoRepository.Create(userInfo));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfo userInfo, string hash)
        {
            return Ok(await _userInfoRepository.Update(userInfo));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userInfoRepository.Delete(id));
        }
    }
}
