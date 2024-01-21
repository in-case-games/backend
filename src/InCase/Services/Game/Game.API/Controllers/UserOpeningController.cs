using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers;

[Route("api/user-opening")]
[ApiController]
public class UserOpeningController(IUserOpeningService openingService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserOpeningResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetAsync(id, cancellation);

        return Ok(ApiResult<UserOpeningResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("roulette")]
    public async Task<IActionResult> GetRoulette(CancellationToken cancellation)
    {
        var response = await openingService.GetAsync(20, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellation, int count = 100)
    {
        var response = await openingService.GetAsync(count, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await openingService.GetAsync(UserId, 100, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("ten/last")]
    public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
    {
        var response = await openingService.GetAsync(UserId, 10, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}/userId")]
    public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetAsync(id, 15, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{id:guid}/userId/admin")]
    public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation, int count = 100)
    {
        var response = await openingService.GetAsync(id, count, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("box/{id:guid}")]
    public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetByBoxIdAsync(UserId, id, 100, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("item/{id:guid}")]
    public async Task<IActionResult> GetByItemId(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetByItemIdAsync(UserId, id, 100, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("box/{id:guid}/roulette")]
    public async Task<IActionResult> GetRouletteByBoxId(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetByBoxIdAsync(id, 20, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("item/{id:guid}/roulette")]
    public async Task<IActionResult> GetRouletteByItemId(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.GetByItemIdAsync(id, 20, cancellation);

        return Ok(ApiResult<List<UserOpeningResponse>>.Ok(response));
    }
}