using Identity.API.Common;
using Identity.API.Filters;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [Route("api/user-restriction")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IUserRestrictionService _userRestrictionService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IUserRestrictionService userRestrictionService)
        {
            _userRestrictionService = userRestrictionService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserRestrictionResponse response = await _userRestrictionService.Get(id);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByUserId(id);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByLogin(login);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByUserId(UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{userId}&{ownerId}")]
        public async Task<IActionResult> GetByIds(Guid userId, Guid ownerId)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.Get(userId, ownerId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("owner/{id}")]
        public async Task<IActionResult> GetByOwnerId(Guid id)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByOwnerId(id);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin()
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByOwnerId(UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/owner")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.Get(userId, UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            List<RestrictionTypeResponse> response = await _userRestrictionService.GetTypes();

            return Ok(ApiResult<List<RestrictionTypeResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(UserRestrictionRequest request)
        {
            UserRestrictionResponse response = await _userRestrictionService.Create(request);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(UserRestrictionRequest request)
        {
            UserRestrictionResponse response = await _userRestrictionService.Update(request);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserRestrictionResponse response = await _userRestrictionService.Delete(id);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }
    }
}
