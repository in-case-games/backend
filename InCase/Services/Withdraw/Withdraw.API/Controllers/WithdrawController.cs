using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public WithdrawController(IWithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawItemRequest request)
        {
            UserHistoryWithdrawResponse response = await _withdrawService
                .WithdrawItemAsync(request, UserId);

            return Ok(ApiResult<UserHistoryWithdrawResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("market/{name}/balance")]
        public async Task<IActionResult> GetMarketBalance(string name)
        {
            decimal balance = await _withdrawService.GetMarketBalanceAsync(name);

            return Ok(ApiResult<object>.OK(new
            {
                balance
            }));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetItemInfo(Guid id)
        {
            ItemInfoResponse response = await _withdrawService.GetItemInfoAsync(id);

            return Ok(ApiResult<ItemInfoResponse>.OK(response));
        }
    }
}
