using Microsoft.AspNetCore.Mvc;
using Support.API.Common;
using Support.API.Filters;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Support.API.Controllers
{
    [Route("api/answer-image")]
    [ApiController]
    public class AnswerImageController : ControllerBase
    {
        private readonly IAnswerImageService _imageService;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AnswerImageController(IAnswerImageService imageService)
        {
            _imageService = imageService;
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetAsync(UserId, id, cancellation);

            return Ok(ApiResult<AnswerImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id:guid}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetAsync(id, cancellation);

            return Ok(ApiResult<AnswerImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("answer/{id:guid}")]
        public async Task<IActionResult> GetByAnswerId(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetByAnswerIdAsync(UserId, id, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("answer/{id:guid}/admin")]
        public async Task<IActionResult> GetByAdminAnswerId(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetByAnswerIdAsync(id, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("topic/{id:guid}")]
        public async Task<IActionResult> GetByTopicId(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetByTopicIdAsync(UserId, id, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("topic/{id:guid}/admin")]
        public async Task<IActionResult> GetByAdminTopicId(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.GetByTopicIdAsync(id, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _imageService.GetByUserIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user/{userId:guid}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await _imageService.GetByUserIdAsync(userId, cancellation);

            return Ok(ApiResult<List<AnswerImageResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [RequestSizeLimit(8388608)]
        [HttpPost]
        public async Task<IActionResult> Post(AnswerImageRequest request, CancellationToken cancellation)
        {
            var response = await _imageService.CreateAsync(UserId, request, cancellation);

            return Ok(ApiResult<AnswerImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<AnswerImageResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id:guid}/admin")]
        public async Task<IActionResult> DeleteByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _imageService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<AnswerImageResponse>.Ok(response));
        }
    }
}
