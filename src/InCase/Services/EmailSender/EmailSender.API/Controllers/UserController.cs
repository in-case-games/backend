using EmailSender.API.Common;
using EmailSender.API.Filters;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace EmailSender.API.Controllers;
[Route("api/user")]
[ApiController]
public class UserController(IUserAdditionalInfoService userService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}/is-notify")]
    public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellationToken)
    {
        var response = await userService.GetByUserIdAsync(id, cancellationToken);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("is-notify")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await userService.GetByUserIdAsync(UserId, cancellationToken);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("is-notify/{isNotify:bool}")]
    public async Task<IActionResult> ChangeNotifyEmail(bool isNotify, CancellationToken cancellationToken)
    {
        var response = await userService.UpdateNotifyEmailAsync(UserId, isNotify, cancellationToken);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Admin, Roles.Owner)]
    [HttpGet("{userId:guid}/is-notify/{isNotify:bool}/admin")]
    public async Task<IActionResult> ChangeNotifyEmailByAdmin(Guid userId, bool isNotify, CancellationToken cancellationToken)
    {
        var response = await userService.UpdateNotifyEmailAsync(userId, isNotify, cancellationToken);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }
}