using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promocode.API.Common;
using Promocode.API.Filters;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Promocode.API.Controllers;
[Route("api/user-promo-codes")]
[ApiController]
public class UserPromoCodesController(IUserPromoCodesService promoCodeService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<List<UserPromoCodeResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await promoCodeService.GetAsync(UserId, 100, cancellation);

        return Ok(ApiResult<List<UserPromoCodeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserPromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await promoCodeService.GetAsync(id, UserId, cancellation);

        return Ok(ApiResult<UserPromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserPromoCodeResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("admin")]
    public async Task<IActionResult> Get(CancellationToken cancellation, int count = 100)
    {
        var response = await promoCodeService.GetAsync(count, cancellation);

        return Ok(ApiResult<List<UserPromoCodeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserPromoCodeResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("user/{userId:guid}/admin")]
    public async Task<IActionResult> GetByAdmin(Guid userId, CancellationToken cancellation, int count = 100)
    {
        var response = await promoCodeService.GetAsync(userId, count, cancellation);

        return Ok(ApiResult<List<UserPromoCodeResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserPromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{id:guid}/admin")]
    public async Task<IActionResult> GetByIdAdmin(Guid id, CancellationToken cancellation)
    {
        var response = await promoCodeService.GetAsync(id, cancellation);

        return Ok(ApiResult<UserPromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserPromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("activate/{name}")]
    public async Task<IActionResult> ActivatePromoCode(string name, CancellationToken cancellation)
    {
        var response = await promoCodeService.ActivateAsync(UserId, name, cancellation);

        return Ok(ApiResult<UserPromoCodeResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserPromoCodeResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("exchange/{name}")]
    public async Task<IActionResult> ExchangePromoCode(string name, CancellationToken cancellation)
    {
        var response = await promoCodeService.ExchangeAsync(UserId, name, cancellation);

        return Ok(ApiResult<UserPromoCodeResponse>.Ok(response));
    }
}