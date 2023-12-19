using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Withdraw.API.Common;
using Withdraw.API.Filters;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;

namespace Withdraw.API.Controllers
{
    [Route("api/withdraw")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        private readonly IWithdrawService _withdrawService;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public WithdrawController(IWithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        [ProducesResponseType(typeof(ApiResult<UserHistoryWithdrawResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawItemRequest request, CancellationToken cancellation)
        {
            var response = await _withdrawService.WithdrawItemAsync(request, UserId, cancellation);

            return Ok(ApiResult<UserHistoryWithdrawResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<BalanceMarketResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("market/{name}/balance")]
        public async Task<IActionResult> GetMarketBalance(string name, CancellationToken cancellation)
        {
            var response = await _withdrawService.GetMarketBalanceAsync(name, cancellation);

            return Ok(ApiResult<BalanceMarketResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ItemInfoResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetItemInfo(Guid id, CancellationToken cancellation)
        {
            var response = await _withdrawService.GetItemInfoAsync(id, cancellation);

            return Ok(ApiResult<ItemInfoResponse>.OK(response));
        }
    }
}
