using Microsoft.AspNetCore.Mvc;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.API.Common;
using Identity.API.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Identity.API.Controllers;

[Route("api/user-additional-info")]
[ApiController]
public class UserAdditionalInfoController(IUserAdditionalInfoService infoService) : Controller
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await infoService.GetAsync(id, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
    {
        var response = await infoService.GetByUserIdAsync(id, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await infoService.GetByUserIdAsync(UserId, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpGet("role/{id:guid}&{userId:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, Guid userId, CancellationToken cancellation)
    {
        var response = await infoService.UpdateRoleAsync(userId, id, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Admin, Roles.Owner)]
    [HttpGet("deletion/date/{userId:guid}")]
    public async Task<IActionResult> UpdateDeletionDate(Guid userId, CancellationToken cancellation, DateTime? date = null)
    {
        var response = await infoService.UpdateDeletionDateAsync(userId, date, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Admin, Roles.Owner)]
    [RequestSizeLimit(8388608)]
    [HttpPut("image/admin")]
    public async Task<IActionResult> UpdateImageByAdmin(UpdateImageRequest request, CancellationToken cancellation)
    {
        var response = await infoService
            .UpdateImageAsync(request, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [RequestSizeLimit(8388608)]
    [HttpPut("image")]
    public async Task<IActionResult> UpdateImage(UpdateImageRequest request, CancellationToken cancellation)
    {
        request.UserId = UserId;

        var response = await infoService.UpdateImageAsync(request, cancellation);

        return Ok(ApiResult<UserAdditionalInfoResponse>.Ok(response));
    }
}