using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promocode.API.Common;
using Promocode.API.Filters;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Promocode.API.Controllers
{
    [Route("api/user-promocodes")]
    [ApiController]
    public class UserPromocodesController(IUserPromocodesService promocodeService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [ProducesResponseType(typeof(ApiResult<List<UserPromocodeResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await promocodeService.GetAsync(UserId, 100, cancellation);

            return Ok(ApiResult<List<UserPromocodeResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await promocodeService.GetAsync(id, UserId, cancellation);

            return Ok(ApiResult<UserPromocodeResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPromocodeResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("admin")]
        public async Task<IActionResult> Get(CancellationToken cancellation, int count = 100)
        {
            var response = await promocodeService.GetAsync(count, cancellation);

            return Ok(ApiResult<List<UserPromocodeResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPromocodeResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId:guid}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid userId, CancellationToken cancellation, int count = 100)
        {
            var response = await promocodeService.GetAsync(userId, count, cancellation);

            return Ok(ApiResult<List<UserPromocodeResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id:guid}/admin")]
        public async Task<IActionResult> GetByIdAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await promocodeService.GetAsync(id, cancellation);

            return Ok(ApiResult<UserPromocodeResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("activate/{name}")]
        public async Task<IActionResult> ActivatePromocode(string name, CancellationToken cancellation)
        {
            var response = await promocodeService.ActivateAsync(UserId, name, cancellation);

            return Ok(ApiResult<UserPromocodeResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("exchange/{name}")]
        public async Task<IActionResult> ExchangePromocode(string name, CancellationToken cancellation)
        {
            var response = await promocodeService.ExchangeAsync(UserId, name, cancellation);

            return Ok(ApiResult<UserPromocodeResponse>.Ok(response));
        }
    }
}
