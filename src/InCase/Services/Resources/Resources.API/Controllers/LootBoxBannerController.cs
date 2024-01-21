using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources.API.Common;
using Resources.API.Filters;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Net;

namespace Resources.API.Controllers;

[Route("api/loot-box-banner")]
[ApiController]
public class LootBoxBannerController(ILootBoxBannerService bannerService) : ControllerBase
{
    [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await bannerService.GetAsync(cancellation);

        return Ok(ApiResult<List<LootBoxBannerResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<LootBoxBannerResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("active/{isActive:bool}")]
    public async Task<IActionResult> GetByIsActive(CancellationToken cancellation, bool isActive = true)
    {
        var response = await bannerService.GetAsync(isActive, cancellation);

        return Ok(ApiResult<List<LootBoxBannerResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await bannerService.GetAsync(id, cancellation);

        return Ok(ApiResult<LootBoxBannerResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("box/{id:guid}")]
    public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
    {
        var response = await bannerService.GetByBoxIdAsync(id, cancellation);

        return Ok(ApiResult<LootBoxBannerResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
    [RequestSizeLimit(8388608)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpPost]
    public async Task<IActionResult> Post(LootBoxBannerRequest request, CancellationToken cancellation)
    {
        var response = await bannerService.CreateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxBannerResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
    [RequestSizeLimit(8388608)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpPut]
    public async Task<IActionResult> Put(LootBoxBannerRequest request, CancellationToken cancellation)
    {
        var response = await bannerService.UpdateAsync(request, cancellation);

        return Ok(ApiResult<LootBoxBannerResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<LootBoxBannerResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var response = await bannerService.DeleteAsync(id, cancellation);

        return Ok(ApiResult<LootBoxBannerResponse>.Ok(response));
    }
}