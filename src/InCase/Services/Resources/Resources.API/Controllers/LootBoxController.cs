using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers;

[Route("api/loot-box")]
[ApiController]
public class LootBoxController(ILootBoxService boxService) : ControllerBase
{
    [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await boxService.GetAsync(cancellation);

        return Ok(ApiResult<List<LootBoxResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<LootBoxResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("game/{id:guid}")]
    public async Task<IActionResult> GetByGameId(Guid id, CancellationToken cancellation)
    {
        var response = await boxService.GetByGameIdAsync(id, cancellation);

        return Ok(ApiResult<List<LootBoxResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await boxService.GetAsync(id, cancellation);

        return Ok(ApiResult<LootBoxResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> Get(string name, CancellationToken cancellation)
    {
        var response = await boxService.GetAsync(name, cancellation);

        return Ok(ApiResult<LootBoxResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [RequestSizeLimit(8388608)]
    [HttpPost]
    public async Task<IActionResult> Post(LootBoxRequest request, CancellationToken cancellation)
    {
        var response = await boxService.CreateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [RequestSizeLimit(8388608)]
    [HttpPut]
    public async Task<IActionResult> Put(LootBoxRequest request, CancellationToken cancellation)
    {
        var response = await boxService.UpdateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var response = await boxService.DeleteAsync(id, cancellation);

        return Ok(ApiResult<LootBoxResponse>.Ok(response));
    }
}