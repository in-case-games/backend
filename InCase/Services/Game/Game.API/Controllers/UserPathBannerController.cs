using Game.API.Common;
using Game.API.Filters;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/user-path-banner")]
    [ApiController]
    public class UserPathBannerController : ControllerBase
    {
        private readonly IUserPathBannerService _pathBannerService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserPathBannerController(IUserPathBannerService pathBannerService)
        {
            _pathBannerService = pathBannerService;
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserPathBannerResponse> response = await _pathBannerService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<UserPathBannerResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserPathBannerResponse response = await _pathBannerService.GetByIdAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("box/{id}")]
        public async Task<IActionResult> GetByBoxId(Guid id)
        {
            UserPathBannerResponse response = await _pathBannerService.GetByBoxIdAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetByItemId(Guid id)
        {
            List<UserPathBannerResponse> response = await _pathBannerService
                .GetByItemIdAsync(id, UserId);

            return Ok(ApiResult<List<UserPathBannerResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(UserPathBannerRequest request)
        {
            request.UserId = UserId;
            
            UserPathBannerResponse response = await _pathBannerService.CreateAsync(request);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(UserPathBannerRequest request)
        {
            request.UserId = UserId;

            UserPathBannerResponse response = await _pathBannerService.UpdateAsync(request);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserPathBannerResponse response = await _pathBannerService.DeleteAsync(id, UserId);

            return Ok(ApiResult<UserPathBannerResponse>.OK(response));
        }
    }
}
