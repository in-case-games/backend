using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers;
[Route("api/user-additional-info")]
[ApiController]
public class UserAdditionalInfoController(IUserAdditionalInfoService infoService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<GuestModeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("guest-mode")]
    public async Task<IActionResult> GetGuestMode(CancellationToken cancellation)
    {
        var response = await infoService.GetGuestModeAsync(UserId, cancellation);

        return Ok(ApiResult<GuestModeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<BalanceResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(CancellationToken cancellation)
    {
        var response = await infoService.GetBalanceAsync(UserId, cancellation);

        return Ok(ApiResult<BalanceResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<BalanceResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{userId:guid}/balance")]
    public async Task<IActionResult> GetBalanceByAdmin(Guid userId, CancellationToken cancellation)
    {
        var response = await infoService.GetBalanceAsync(userId, cancellation);

        return Ok(ApiResult<BalanceResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<BalanceResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpGet("{userId:guid}/balance/{balance:decimal}/owner")]
    public async Task<IActionResult> ChangeBalanceByOwner(Guid userId, decimal balance, CancellationToken cancellation)
    {
        var response = await infoService.ChangeBalanceByOwnerAsync(userId, balance, cancellation);

        return Ok(ApiResult<BalanceResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<GuestModeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpPut("guest-mode")]
    public async Task<IActionResult> SwitchGuestMode(CancellationToken cancellation)
    {
        var response = await infoService.ChangeGuestModeAsync(UserId, cancellation);

        return Ok(ApiResult<GuestModeResponse>.Ok(response));
    }
}