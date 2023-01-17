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
        public async Task<UserAdditionalInfo> GetInfo(Guid id)
        {
            return await _userInfoRepository.GetInfo(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInfo(UserAdditionalInfoDto userInfo)
        {
            var temp = new UserAdditionalInfo()
            {
                Id = userInfo.Id,
                UserId = userInfo.UserId,
                UserRoleId = userInfo.UserRoleId,
                UserAbleToPay = userInfo.UserAbleToPay,
                UserAge = userInfo.UserAge,
                UserBalance = userInfo.UserBalance,
            };
            return Ok(await _userInfoRepository.CreateInfo(temp));
        }

        [HttpPost("UpdateInfo")]
        public async Task<IActionResult> UpdateInfo(UserAdditionalInfoDto userInfo)
        {
            var temp = new UserAdditionalInfo()
            {
                Id = userInfo.Id,
                UserId = userInfo.UserId,
                UserRoleId = userInfo.UserRoleId,
                UserAbleToPay = userInfo.UserAbleToPay,
                UserAge = userInfo.UserAge,
                UserBalance = userInfo.UserBalance,
            };
            return Ok(await _userInfoRepository.UpdateInfo(temp));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInfo(Guid id)
        {
            return Ok(await _userInfoRepository.DeleteInfo(id));
        }
    }
}
