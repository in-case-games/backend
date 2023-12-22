using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/user-path-banner")]
    [ApiController]
    public class UserPathBannerController : ControllerBase
    {
        private readonly IUserPathBannerService _pathService;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserPathBannerController(IUserPathBannerService pathService)
        {
            _pathService = pathService;
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPathBannerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _pathService.GetByUserIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<UserPathBannerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _pathService.GetByIdAsync(id, UserId, cancellation);

            return Ok(ApiResult<UserPathBannerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id:guid}")]
        public async Task<IActionResult> GetByBoxId(Guid id, CancellationToken cancellation)
        {
            var response = await _pathService.GetByBoxIdAsync(id, UserId, cancellation);

            return Ok(ApiResult<UserPathBannerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPathBannerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id:guid}")]
        public async Task<IActionResult> GetByItemId(Guid id, CancellationToken cancellation)
        {
            var response = await _pathService.GetByItemIdAsync(id, UserId, cancellation);

            return Ok(ApiResult<List<UserPathBannerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(UserPathBannerRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;
            
            var response = await _pathService.CreateAsync(request, cancellation);

            return Ok(ApiResult<UserPathBannerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(UserPathBannerRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;

            var response = await _pathService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<UserPathBannerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _pathService.DeleteAsync(id, UserId, cancellation);

            return Ok(ApiResult<UserPathBannerResponse>.Ok(response));
        }
    }
}
