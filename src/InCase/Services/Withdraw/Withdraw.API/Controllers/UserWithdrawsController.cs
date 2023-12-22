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
    [Route("api/user-withdraws")]
    [ApiController]
    public class UserWithdrawsController : ControllerBase
    {
        private readonly IUserWithdrawsService _userWithdrawsService;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserWithdrawsController(IUserWithdrawsService userWithdrawsService)
        {
            _userWithdrawsService = userWithdrawsService;
        }

        [ProducesResponseType(typeof(ApiResult<UserHistoryWithdrawResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _userWithdrawsService.GetAsync(id, cancellation);

            return Ok(ApiResult<UserHistoryWithdrawResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserHistoryWithdrawResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
        {
            var response = await _userWithdrawsService.GetAsync(id, 100, cancellation);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserHistoryWithdrawResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("100/last")]
        public async Task<IActionResult> GetLast100Withdraw(CancellationToken cancellation)
        {
            var response = await _userWithdrawsService.GetAsync(100, cancellation);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserHistoryWithdrawResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _userWithdrawsService.GetAsync(UserId, 100, cancellation);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserHistoryWithdrawResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> Get(CancellationToken cancellation, int count = 100)
        {
            var response = await _userWithdrawsService.GetAsync(count, cancellation);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.Ok(response));
        }


        [ProducesResponseType(typeof(ApiResult<List<UserHistoryWithdrawResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId:guid}/admin")]
        public async Task<IActionResult> Get(Guid userId, CancellationToken cancellation, int count = 100)
        {
            var response = await _userWithdrawsService.GetAsync(userId, count, cancellation);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id:guid}/transfer")]
        public async Task<IActionResult> TransferToInventory(Guid id, CancellationToken cancellation)
        {
            var response = await _userWithdrawsService.TransferAsync(id, UserId, cancellation);

            return Ok(ApiResult<UserInventoryResponse>.Ok(response));
        }
    }
}
