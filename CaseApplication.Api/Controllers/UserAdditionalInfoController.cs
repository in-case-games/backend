using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
        public async Task<UserAdditionalInfo> GetInfo(Guid userId)
        {
            return await _userInfoRepository.GetInfo(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInfo(UserAdditionalInfo userInfo)
        {
            return Ok(await _userInfoRepository.CreateInfo(userInfo));
        }

        [HttpPost("UpdateInfo")]
        public async Task<IActionResult> UpdateInfo(UserAdditionalInfo userInfo, string hash)
        {
            return Ok(await _userInfoRepository.UpdateInfo(userInfo));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInfo(Guid infoId, User userRemoving)
        {
            return Ok(await _userInfoRepository.DeleteInfo(infoId));
        }
    }
}
