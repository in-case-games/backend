using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/user-opening")]
    [ApiController]
    public class UserOpeningController : ControllerBase
    {
        private readonly IUserOpeningService _openingService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserOpeningController(IUserOpeningService openingService)
        {
            _openingService = openingService;
        }

        [ProducesResponseType(typeof(ApiResult<UserOpeningResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserOpeningResponse response = await _openingService
                .GetAsync(id);

            return Ok(ApiResult<UserOpeningResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("roulete")]
        public async Task<IActionResult> GetRoulete()
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet]
        public async Task<IActionResult> Get(int count = 100)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(count);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("ten/last")]
        public async Task<IActionResult> GetByUserId()
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(UserId, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, int count = 100)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(id, count);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByBoxIdAsync(UserId, id, 100);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByItemIdAsync(UserId, id, 100);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}/roulete")]
        public async Task<IActionResult> GetRouleteByBoxId(Guid id)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByBoxIdAsync(id, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("item/{id}/roulete")]
        public async Task<IActionResult> GetRouleteByItemId(Guid id)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByBoxIdAsync(id, 10);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }
    }
}
