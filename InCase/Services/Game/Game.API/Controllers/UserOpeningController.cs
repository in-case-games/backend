using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/user-opening")]
    [ApiController]
    public class UserOpeningController : ControllerBase
    {
        private readonly IUserOpeningService _userOpeningService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserOpeningController(IUserOpeningService userOpeningService)
        {
            _userOpeningService = userOpeningService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserOpeningResponse response = await _userOpeningService
                .GetAsync(id);

            return Ok(ApiResult<UserOpeningResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("roulete")]
        public async Task<IActionResult> GetRoulete()
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetAsync(10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet]
        public async Task<IActionResult> Get(int count = 100)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetAsync(count);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("ten/last")]
        public async Task<IActionResult> GetByUserId()
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetAsync(UserId, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, int count = 100)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetAsync(id, count);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetByBoxIdAsync(UserId, id, 100);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetByItemIdAsync(UserId, id, 100);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("box/{id}/roulete")]
        public async Task<IActionResult> GetRouleteByBoxId(Guid id)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetByBoxIdAsync(id, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("item/{id}/roulete")]
        public async Task<IActionResult> GetRouleteByItemId(Guid id)
        {
            List<UserOpeningResponse> response = await _userOpeningService
                .GetByBoxIdAsync(id, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }
    }
}
