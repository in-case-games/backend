using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Withdraw.API.Common;
using Withdraw.API.Filters;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;

namespace Withdraw.API.Controllers
{
    [Route("api/user-inventory")]
    [ApiController]
    public class UserInventoryController : ControllerBase
    {
        private readonly IUserInventoryService _userInventoryService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserInventoryController(IUserInventoryService userInventoryService)
        {
            _userInventoryService = userInventoryService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserInventoryResponse response = await _userInventoryService.GetByIdAsync(id);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(userId, 100);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(UserId);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin/{count}")]
        public async Task<IActionResult> Get(Guid userId, int count)
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(userId, count);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/exchange/{itemId}")]
        public async Task<IActionResult> Exchange(Guid id, Guid itemId)
        {
            UserInventoryResponse response = await _userInventoryService
                .ExchangeAsync(id, itemId, UserId);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/sell")]
        public async Task<IActionResult> Sell(Guid id)
        {
            decimal response = await _userInventoryService
                .SellAsync(id, UserId);

            return Ok(ApiResult<object>.OK(new
            {
                cost = response
            }));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("last/sell/{itemId}")]
        public async Task<IActionResult> SellLastItem(Guid itemId)
        {
            decimal response = await _userInventoryService
                .SellLastAsync(itemId, UserId);

            return Ok(ApiResult<object>.OK(new
            {
                cost = response
            }));
        }
    }
}
