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
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserPathBannerController(IUserPathBannerService pathService)
        {
            _pathService = pathService;
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPathBannerResponse>>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserPathBannerResponse> response = await _pathService
                .GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<UserPathBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserPathBannerResponse response = await _pathService
                .GetByIdAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            UserPathBannerResponse response = await _pathService
                .GetByBoxIdAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserPathBannerResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<UserPathBannerResponse> response = await _pathService
                .GetByItemIdAsync(id, UserId);

            return Ok(ApiResult<List<UserPathBannerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(UserPathBannerRequest request)
        {
            request.UserId = UserId;
            
            UserPathBannerResponse response = await _pathService
                .CreateAsync(request);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(UserPathBannerRequest request)
        {
            request.UserId = UserId;

            UserPathBannerResponse response = await _pathService
                .UpdateAsync(request);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserPathBannerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserPathBannerResponse response = await _pathService
                .DeleteAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }
    }
}
