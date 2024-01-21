using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Payment.API.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserPaymentsResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpPost("top-up")]
    public async Task<IActionResult> TopUpBalance(GameMoneyTopUpResponse request, CancellationToken cancellation)
    {
        var response = await paymentService.TopUpBalanceAsync(request, cancellation);

        return Ok(ApiResult<UserPaymentsResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<PaymentBalanceResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.Owner, Roles.Bot)]
    [HttpGet("balance/{currency}")]
    public async Task<IActionResult> GetBalance(string currency, CancellationToken cancellation)
    {
        var response = await paymentService.GetPaymentBalanceAsync(currency, cancellation);

        return Ok(ApiResult<PaymentBalanceResponse>.Ok(response));
    }

    [ProducesResponseType(typeof(ApiResult<HashOfDataForDepositResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpGet("top-up/signature")]
    public IActionResult GetSignatureForDeposit()
    {
        var response = paymentService.GetHashOfDataForDeposit(UserId);

        return Ok(ApiResult<HashOfDataForDepositResponse>.Ok(response));
    }
}