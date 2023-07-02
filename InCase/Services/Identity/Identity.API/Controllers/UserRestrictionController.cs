using Identity.API.Common;
using Identity.API.Filters;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserRestrictionResponse response = await _userRestrictionService.GetAsync(id);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByUserIdAsync(id);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByLoginAsync(login);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{userId}&{ownerId}")]
        public async Task<IActionResult> GetByIds(Guid userId, Guid ownerId)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetAsync(userId, ownerId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("owner/{id}")]
        public async Task<IActionResult> GetByOwnerId(Guid id)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByOwnerIdAsync(id);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin()
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetByOwnerIdAsync(UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRestrictionResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/owner")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            List<UserRestrictionResponse> response = await _userRestrictionService.GetAsync(userId, UserId);

            return Ok(ApiResult<List<UserRestrictionResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<RestrictionTypeResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            List<RestrictionTypeResponse> response = await _userRestrictionService.GetTypesAsync();

            return Ok(ApiResult<List<RestrictionTypeResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Post(UserRestrictionRequest request)
        {
            UserRestrictionResponse response = await _userRestrictionService.CreateAsync(request);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Put(UserRestrictionRequest request)
        {
            UserRestrictionResponse response = await _userRestrictionService.UpdateAsync(request);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserRestrictionResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserRestrictionResponse response = await _userRestrictionService.DeleteAsync(id);

            return Ok(ApiResult<UserRestrictionResponse>.OK(response));
        }
    }
}
