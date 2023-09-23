using Infrastructure.MassTransit.User;
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

        [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserInventoryResponse response = await _userInventoryService.GetByIdAsync(id);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(id, 100);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(UserId);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin")]
        public async Task<IActionResult> Get(Guid userId, int count = 100)
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .GetAsync(userId, count);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(UserInventoryTemplate request)
        {
            UserInventoryResponse response = await _userInventoryService.CreateAsync(request);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserInventoryResponse>>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut("exchange")]
        public async Task<IActionResult> Exchange(ExchangeItemRequest request)
        {
            List<UserInventoryResponse> response = await _userInventoryService
                .ExchangeAsync(request, UserId);

            return Ok(ApiResult<List<UserInventoryResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SellItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/sell")]
        public async Task<IActionResult> Sell(Guid id)
        {
            SellItemResponse response = await _userInventoryService
                .SellAsync(id, UserId);

            return Ok(ApiResult<SellItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SellItemResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("last/sell/{itemId}")]
        public async Task<IActionResult> SellLastItem(Guid itemId)
        {
            SellItemResponse response = await _userInventoryService
                .SellLastAsync(itemId, UserId);

            return Ok(ApiResult<SellItemResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserInventoryResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserInventoryResponse response = await _userInventoryService.DeleteAsync(id);

            return Ok(ApiResult<UserInventoryResponse>.OK(response));
        }
    }
}
