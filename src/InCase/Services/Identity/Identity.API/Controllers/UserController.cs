using Identity.API.Common;
using Identity.API.Filters;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Identity.API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await userService.GetAsync(id, cancellation);

        return Ok(ApiResult<UserResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await userService.GetAsync(UserId, cancellation);

        return Ok(ApiResult<UserResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("login/{login}")]
    public async Task<IActionResult> Get(string login, CancellationToken cancellation)
    {
        var response = await userService.GetAsync(login, cancellation);

        return Ok(ApiResult<UserResponse>.Ok(response));
    }
}