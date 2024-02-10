using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers;
[Route("api/loot-box-group")]
[ApiController]
public class LootBoxGroupController(ILootBoxGroupService groupService) : ControllerBase
{
    [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await groupService.GetAsync(cancellation);

        return Ok(ApiResult<List<LootBoxGroupResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await groupService.GetAsync(id, cancellation);

        return Ok(ApiResult<LootBoxGroupResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("game/{id:guid}")]
    public async Task<IActionResult> GetByGameId(Guid id, CancellationToken cancellation)
    {
        var response = await groupService.GetByGameIdAsync(id, cancellation);

        return Ok(ApiResult<List<LootBoxGroupResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("box/{id:guid}")]
    public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
    {
        var response = await groupService.GetByBoxIdAsync(id, cancellation);

        return Ok(ApiResult<List<LootBoxGroupResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<LootBoxGroupResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("group/{id:guid}")]
    public async Task<IActionResult> GetByGroupId(Guid id, CancellationToken cancellation)
    {
        var response = await groupService.GetByGroupIdAsync(id, cancellation);

        return Ok(ApiResult<List<LootBoxGroupResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpPost]
    public async Task<IActionResult> Post(LootBoxGroupRequest request, CancellationToken cancellation)
    {
        var response = await groupService.CreateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxGroupResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpPut]
    public async Task<IActionResult> Put(LootBoxGroupRequest request, CancellationToken cancellation)
    {
        var response = await groupService.UpdateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxGroupResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxGroupResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var response = await groupService.DeleteAsync(id, cancellation);

        return Ok(ApiResult<LootBoxGroupResponse>.Ok(response));
    }
}