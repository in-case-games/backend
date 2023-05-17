using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Security.Claims;

namespace Payment.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //TODO Delete method, check background service and top up balance
        [AllowAnonymous]
        [HttpPost("top-up")]
        public async Task<IActionResult> TopUpBalance(GameMoneyTopUpResponse request)
        {
            UserPaymentsResponse response = await _paymentService.TopUpBalanceAsync(request);

            return Ok(ApiResult<UserPaymentsResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner, Roles.Bot)]
        [HttpGet("balance/{currency}")]
        public async Task<IActionResult> GetBalance(string currency)
        {
            decimal balance = await _paymentService.GetPaymentBalanceAsync(currency);

            return Ok(ApiResult<object>.OK(new { balance }));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("top-up/signature")]
        public IActionResult GetSignatureForDeposit()
        {
            string hmac = _paymentService.GetHashOfDataForDeposit(UserId);

            return Ok(ApiResult<object>.OK(new{ hmac }));
        }
    }
}
