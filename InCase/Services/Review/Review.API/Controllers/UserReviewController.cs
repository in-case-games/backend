using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review.API.Common;
using Review.API.Filters;
using Review.BLL.Interfaces;
using Review.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Review.API.Controllers
{
    [Route("api/user-review")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly IUserReviewService _userReviewService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserReviewController(IUserReviewService userReviewService)
        {
            _userReviewService = userReviewService;
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserReviewResponse> response = await _userReviewService.GetAsync(isOnlyApproved: true);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserReviewResponse response = await _userReviewService.GetAsync(id, isOnlyApproved: true);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            List<UserReviewResponse> response = await _userReviewService
                .GetByUserIdAsync(id, isOnlyApproved: true);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
        {
            List<UserReviewResponse> response = await _userReviewService.GetAsync(isOnlyApproved: false);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            UserReviewResponse response = await _userReviewService.GetAsync(id, isOnlyApproved: false);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{id}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid id)
        {
            List<UserReviewResponse> response = await _userReviewService
                .GetByUserIdAsync(id, isOnlyApproved: false);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId()
        {
            List<UserReviewResponse> response = await _userReviewService
                .GetByUserIdAsync(UserId, isOnlyApproved: true);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            UserReviewResponse response = await _userReviewService.ApproveReviewAsync(id);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/denied")]
        public async Task<IActionResult> Denied(Guid id)
        {
            UserReviewResponse response = await _userReviewService.ApproveReviewAsync(id);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(UserReviewRequest request)
        {
            UserReviewResponse response = await _userReviewService.CreateAsync(request);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(UserReviewRequest request)
        {
            UserReviewResponse response = await _userReviewService.UpdateAsync(request);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserReviewResponse response = await _userReviewService.DeleteAsync(UserId, id);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            UserReviewResponse response = await _userReviewService.DeleteAsync(id);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }
    }
}
