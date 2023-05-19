using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserWithdrawsController(IUserWithdrawsService userWithdrawsService)
        {
            _userWithdrawsService = userWithdrawsService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserHistoryWithdrawResponse response = await _userWithdrawsService.Get(id);

            return Ok(ApiResult<UserHistoryWithdrawResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            List<UserHistoryWithdrawResponse> response = await _userWithdrawsService
                .Get(userId, 100);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("100/last")]
        public async Task<IActionResult> GetLast100Withdraw()
        {
            List<UserHistoryWithdrawResponse> response = await _userWithdrawsService
                .Get(100);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserHistoryWithdrawResponse> response = await _userWithdrawsService
                .Get(UserId, 100);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin/{count}")]
        public async Task<IActionResult> Get(int count)
        {
            List<UserHistoryWithdrawResponse> response = await _userWithdrawsService
                .Get(count);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin/{count}")]
        public async Task<IActionResult> Get(Guid userId, int count)
        {
            List<UserHistoryWithdrawResponse> response = await _userWithdrawsService
                .Get(userId, count);

            return Ok(ApiResult<List<UserHistoryWithdrawResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/transfer")]
        public async Task<IActionResult> TransferToInventory(Guid id)
        {
            UserInventoryResponse response = await _userWithdrawsService
                .Transfer(id, UserId);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }
    }
}
