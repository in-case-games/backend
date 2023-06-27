using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IUserAdditionalInfoService _userInfoService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [ProducesResponseType(typeof(ApiResult<GuestModeResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("guest-mode")]
        public async Task<IActionResult> GetGuestMode()
        {
            GuestModeResponse response = await _userInfoService.GetGuestModeAsync(UserId);

            return Ok(ApiResult<GuestModeResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            BalanceResponse response = await _userInfoService.GetBalanceAsync(UserId);

            return Ok(ApiResult<BalanceResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/balance")]
        public async Task<IActionResult> GetBalanceByAdmin(Guid userId)
        {
            BalanceResponse response = await _userInfoService.GetBalanceAsync(userId);

            return Ok(ApiResult<BalanceResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GuestModeResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut("guest-mode")]
        public async Task<IActionResult> SwitchGuestMode()
        {
            GuestModeResponse response = await _userInfoService.ChangeGuestModeAsync(UserId);

            return Ok(ApiResult<GuestModeResponse>.OK(response));
        }
    }
}
