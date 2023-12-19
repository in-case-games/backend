using Identity.API.Common;
using Identity.API.Filters;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [Route("api/user-restriction")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IUserRestrictionService _restrictionService;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IUserRestrictionService restrictionService)
        {
            _restrictionService = restrictionService;
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetAsync(id, cancellation);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetByUserIdAsync(id, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetByLoginAsync(login, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _restrictionService.GetByUserIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{userId}&{ownerId}")]
        public async Task<IActionResult> GetByIds(Guid userId, Guid ownerId, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetAsync(userId, ownerId, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("owner/{id}")]
        public async Task<IActionResult> GetByOwnerId(Guid id, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetByOwnerIdAsync(id, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellation)
        {
            var response = await _restrictionService.GetByOwnerIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/owner")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await _restrictionService.GetAsync(userId, UserId, cancellation);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<RestrictionTypeResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType(CancellationToken cancellation)
        {
            var response = await _restrictionService.GetTypesAsync(cancellation);

            return Ok(ApiResult<List<RestrictionTypeResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(UserRestrictionRequest request, CancellationToken cancellation)
        {
            request.OwnerId = UserId;

            var response = await _restrictionService.CreateAsync(request, cancellation);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(UserRestrictionRequest request, CancellationToken cancellation)
        {
            request.OwnerId = UserId;

            var response = await _restrictionService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _restrictionService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }
    }
}
