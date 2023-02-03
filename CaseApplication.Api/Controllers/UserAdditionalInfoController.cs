using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid userId)
        {
            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.Get(userId);

            if (userAdditionalInfo != null) {
                return Ok(userAdditionalInfo);
            }

            return NotFound();
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
