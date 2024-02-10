using Microsoft.AspNetCore.Mvc;
using Promocode.API.Common;
using Promocode.API.Filters;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using System.Net;

namespace Promocode.API.Controllers;

[Route("api/promo-code")]
[ApiController]
public class PromoCodeController(IPromoCodeService promoCodeService) : ControllerBase
{
    [ProducesResponseType(typeof(ApiResult<List<PromoCodeResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await promoCodeService.GetAsync(cancellation);

        return Ok(ApiResult<List<PromoCodeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<PromoCodeResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("empty")]
    public async Task<IActionResult> GetEmpty(CancellationToken cancellation)
    {
        var response = await promoCodeService.GetEmptyPromoCodesAsync(cancellation);

        return Ok(ApiResult<List<PromoCodeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<PromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> Get(string name, CancellationToken cancellation)
    {
        var response = await promoCodeService.GetAsync(name, cancellation);

        return Ok(ApiResult<PromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<PromoCodeTypeResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("types")]
    public async Task<IActionResult> GetTypes(CancellationToken cancellation)
    {
        var response = await promoCodeService.GetTypesAsync(cancellation);

        return Ok(ApiResult<List<PromoCodeTypeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<PromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpPost]
    public async Task<IActionResult> Post(PromoCodeRequest request, CancellationToken cancellation)
    {
        var response = await promoCodeService.CreateAsync(request, cancellation);

        return Ok(ApiResult<PromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<PromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpPut]
    public async Task<IActionResult> Put(PromoCodeRequest request, CancellationToken cancellation)
    {
        var response = await promoCodeService.UpdateAsync(request, cancellation);

        return Ok(ApiResult<PromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<PromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var response = await promoCodeService.DeleteAsync(id, cancellation);

        return Ok(ApiResult<PromoCodeResponse>.Ok(response));
    }
}