using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
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
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            UserOpeningResponse response = await _openingService
                .GetAsync(id, cancellation);

            return Ok(ApiResult<UserOpeningResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("roulette")]
        public async Task<IActionResult> GetRoulete(CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(20, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation, int count = 100)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(count, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("ten/last")]
        public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(UserId, 10, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}/userId")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(id, 15, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/userId/admin")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation, int count = 100)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetAsync(id, count, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByBoxIdAsync(UserId, id, 100, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id, CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByItemIdAsync(UserId, id, 100, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("box/{id}/roulette")]
        public async Task<IActionResult> GetRouleteByBoxId(Guid id, CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByBoxIdAsync(id, 20, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserOpeningResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("item/{id}/roulette")]
        public async Task<IActionResult> GetRouleteByItemId(Guid id, CancellationToken cancellation)
        {
            List<UserOpeningResponse> response = await _openingService
                .GetByItemIdAsync(id, 20, cancellation);

            return Ok(ApiResult<List<UserOpeningResponse>>.OK(response));
        }
    }
}
