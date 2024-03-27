using Infrastructure.MassTransit.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Withdraw.API.Common;
using Withdraw.API.Filters;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;

namespace Withdraw.API.Controllers;
[Route("api/user-inventory")]
[ApiController]
public class UserInventoryController(IUserInventoryService userInventoryService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
    {
        var response = await userInventoryService.GetByIdAsync(id, cancellation);

        return Ok(ApiResult<UserInventoryResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
    {
        var response = await userInventoryService.GetAsync(id, 100, cancellation);

        return Ok(ApiResult<List<UserInventoryResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await userInventoryService.GetAsync(UserId, cancellation);

        return Ok(ApiResult<List<UserInventoryResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{userId:guid}/admin")]
    public async Task<IActionResult> Get(Guid userId, CancellationToken cancellation, int count = 100)
    {
        var response = await userInventoryService.GetAsync(userId, count, cancellation);

        return Ok(ApiResult<List<UserInventoryResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpPost]
    public async Task<IActionResult> Post(UserInventoryTemplate request, CancellationToken cancellation)
    {
        var response = await userInventoryService.CreateAsync(request, cancellation);

        return Ok(ApiResult<UserInventoryResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpPut("exchange")]
    public async Task<IActionResult> Exchange(ExchangeItemRequest request, CancellationToken cancellation)
    {
        var response = await userInventoryService.ExchangeAsync(request, UserId, cancellation);

        return Ok(ApiResult<List<UserInventoryResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<SellItemResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}/sell")]
    public async Task<IActionResult> Sell(Guid id, CancellationToken cancellation)
    {
        var response = await userInventoryService.SellAsync(id, UserId, cancellation);

        return Ok(ApiResult<SellItemResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<SellItemResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("last/sell/{itemId:guid}")]
    public async Task<IActionResult> SellLastItem(Guid itemId, CancellationToken cancellation)
    {
        var response = await userInventoryService.SellLastAsync(itemId, UserId, cancellation);

        return Ok(ApiResult<SellItemResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var response = await userInventoryService.DeleteAsync(id, cancellation);

        return Ok(ApiResult<UserInventoryResponse>.Ok(response));
    }
}