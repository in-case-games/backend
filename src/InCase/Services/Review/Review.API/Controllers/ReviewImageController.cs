using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.API.Common;
using Review.API.Filters;
using Review.BLL.Interfaces;
using Review.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Review.API.Controllers
{
    [Route("api/review-image")]
    [ApiController]
    public class ReviewImageController : ControllerBase
    {
        private readonly IReviewImageService _reviewImageService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ReviewImageController(IReviewImageService reviewImageService)
        {
            _reviewImageService = reviewImageService;
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService.GetAsync(isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService.GetAsync(isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            ReviewImageResponse response = await _reviewImageService.GetAsync(id, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            ReviewImageResponse response = await _reviewImageService.GetAsync(id, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(userId, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(UserId, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(userId, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("review/{id}")]
        public async Task<IActionResult> GetByReviewId(Guid id, CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByReviewIdAsync(id, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("review/{id}/admin")]
        public async Task<IActionResult> GetByAdminReviewId(Guid id, CancellationToken cancellation)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByReviewIdAsync(id, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [RequestSizeLimit(8388608)]
        [HttpPost]
        public async Task<IActionResult> Post(ReviewImageRequest request, CancellationToken cancellation)
        {
            ReviewImageResponse response = await _reviewImageService.CreateAsync(UserId, request, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            ReviewImageResponse response = await _reviewImageService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteAdmin(Guid id, CancellationToken cancellation)
        {
            ReviewImageResponse response = await _reviewImageService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }
    }
}
