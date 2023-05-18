using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promocode.API.Common;
using Promocode.API.Filters;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using System.Security.Claims;

namespace Promocode.API.Controllers
{
    [Route("api/user-promocodes")]
    [ApiController]
    public class UserPromocodesController : ControllerBase
    {
        private readonly IUserPromocodesService _userPromocodesService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserPromocodesController(IUserPromocodesService userPromocodesService)
        {
            _userPromocodesService = userPromocodesService;
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserHistoryPromocodeResponse> response = await _userPromocodesService
                .GetAsync(UserId, 100);

            return Ok(ApiResult<List<UserHistoryPromocodeResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserHistoryPromocodeResponse response = await _userPromocodesService
                .GetAsync(id, UserId);

            return Ok(ApiResult<UserHistoryPromocodeResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("admin")]
        public async Task<IActionResult> Get(int count = 100)
        {
            List<UserHistoryPromocodeResponse> response = await _userPromocodesService
                .GetAsync(count);

            return Ok(ApiResult<List<UserHistoryPromocodeResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid userId, int count = 100)
        {
            List<UserHistoryPromocodeResponse> response = await _userPromocodesService
                .GetAsync(userId, count);

            return Ok(ApiResult<List<UserHistoryPromocodeResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin/{id}")]
        public async Task<IActionResult> GetByIdAdmin(Guid userId, Guid id)
        {
            UserHistoryPromocodeResponse response = await _userPromocodesService
                .GetAsync(id, userId);

            return Ok(ApiResult<UserHistoryPromocodeResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("activate/promocode/{name}")]
        public async Task<IActionResult> ActivatePromocode(string name)
        {
            UserHistoryPromocodeResponse response = await _userPromocodesService
                .ActivateAsync(UserId, name);

            return Ok(ApiResult<UserHistoryPromocodeResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("exchange/promocode/{name}")]
        public async Task<IActionResult> ExchangePromocode(string name)
        {
            UserHistoryPromocodeResponse response = await _userPromocodesService
                .ExchangeAsync(UserId, name);

            return Ok(ApiResult<UserHistoryPromocodeResponse>.OK(response));
        }
    }
}
