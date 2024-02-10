using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers;
[Route("api/loot-box-opening")]
[ApiController]
public class LootBoxOpeningController(ILootBoxOpeningService openingService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.OpenBox(UserId, id, cancellation);

        return Ok(ApiResult<GameItemResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<GameItemResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}/virtual")]
    public async Task<IActionResult> GetVirtual(Guid id, CancellationToken cancellation)
    {
        var response = await openingService.OpenVirtualBox(UserId, id, cancellation);

        return Ok(ApiResult<GameItemResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<GameItemBigOpenResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}&{count:int}/virtual")]
    public async Task<IActionResult> GetVirtual(Guid id, int count, CancellationToken cancellation)
    {
        var response = await openingService.OpenVirtualBox(UserId, id, count, cancellation: cancellation);

        return Ok(ApiResult<List<GameItemBigOpenResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<GameItemBigOpenResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{id:guid}&{count:int}/virtual/admin")]
    public async Task<IActionResult> GetVirtualByAdmin(Guid id, int count, CancellationToken cancellation)
    {
        var response = await openingService.OpenVirtualBox(UserId, id, count, isAdmin: true, cancellation);

        return Ok(ApiResult<List<GameItemBigOpenResponse>>.Ok(response));
    }
}