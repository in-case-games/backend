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
        private readonly IUserAdditionalInfoService _infoService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoService infoService)
        {
            _infoService = infoService;
        }

        [ProducesResponseType(typeof(ApiResult<GuestModeResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("guest-mode")]
        public async Task<IActionResult> GetGuestMode(CancellationToken cancellation)
        {
            GuestModeResponse response = await _infoService.GetGuestModeAsync(UserId, cancellation);

            return Ok(ApiResult<GuestModeResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance(CancellationToken cancellation)
        {
            BalanceResponse response = await _infoService.GetBalanceAsync(UserId, cancellation);

            return Ok(ApiResult<BalanceResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/balance")]
        public async Task<IActionResult> GetBalanceByAdmin(Guid userId, CancellationToken cancellation)
        {
            BalanceResponse response = await _infoService.GetBalanceAsync(userId, cancellation);

            return Ok(ApiResult<BalanceResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId}/balance/{balance}/owner")]
        public async Task<IActionResult> ChangeBalanceByOwner(Guid userId, decimal balance, CancellationToken cancellation)
        {
            BalanceResponse response = await _infoService
                .ChangeBalanceByOwnerAsync(userId, balance, cancellation);

            return Ok(ApiResult<BalanceResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<GuestModeResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut("guest-mode")]
        public async Task<IActionResult> SwitchGuestMode(CancellationToken cancellation)
        {
            GuestModeResponse response = await _infoService.ChangeGuestModeAsync(UserId, cancellation);

            return Ok(ApiResult<GuestModeResponse>.OK(response));
        }
    }
}
