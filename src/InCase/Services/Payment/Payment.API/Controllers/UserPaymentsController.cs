﻿using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Payment.API.Controllers;

[Route("api/user-payments")]
[ApiController]
public class UserPaymentsController(IUserPaymentsService paymentsService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<List<UserPaymentsResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var response = await paymentsService.GetAsync(UserId, 100, cancellation);

        return Ok(ApiResult<List<UserPaymentsResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<UserPaymentsResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellation)
    {
        var response = await paymentsService.GetByIdAsync(id, UserId, cancellation);

        return Ok(ApiResult<UserPaymentsResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserPaymentsResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("admin")]
    public async Task<IActionResult> GetByAdmin(CancellationToken cancellation, int count = 100)
    {
        var response = await paymentsService.GetAsync(count, cancellation);

        return Ok(ApiResult<List<UserPaymentsResponse>>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<List<UserPaymentsResponse>>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.AdminOwnerBot)]
    [HttpGet("{userId:guid}/admin")]
    public async Task<IActionResult> GetByUserIdAdmin(Guid userId, CancellationToken cancellation, int count = 100)
    {
        var response = await paymentsService.GetAsync(userId, count, cancellation);

        return Ok(ApiResult<List<UserPaymentsResponse>>.Ok(response));
    }
}