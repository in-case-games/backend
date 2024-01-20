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
    public class ReviewImageController(IReviewImageService reviewImageService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await reviewImageService.GetAsync(isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellation)
        {
            var response = await reviewImageService.GetAsync(isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetAsync(id, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id:guid}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetAsync(id, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetByUserIdAsync(id, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
        {
            var response = await reviewImageService.GetByUserIdAsync(UserId, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId:guid}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetByUserIdAsync(userId, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("review/{id:guid}")]
        public async Task<IActionResult> GetByReviewId(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetByReviewIdAsync(id, isOnlyApproved: true, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<ReviewImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("review/{id:guid}/admin")]
        public async Task<IActionResult> GetByAdminReviewId(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.GetByReviewIdAsync(id, isOnlyApproved: false, cancellation);

            return Ok(ApiResult<List<ReviewImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [RequestSizeLimit(8388608)]
        [HttpPost]
        public async Task<IActionResult> Post(ReviewImageRequest request, CancellationToken cancellation)
        {
            var response = await reviewImageService.CreateAsync(UserId, request, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<ReviewImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id:guid}/admin")]
        public async Task<IActionResult> DeleteAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await reviewImageService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<ReviewImageResponse>.Ok(response));
        }
    }
}
