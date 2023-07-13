using Microsoft.AspNetCore.Http;
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
        private readonly IAnswerImageService _answerImageService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AnswerImageController(IAnswerImageService answerImageService)
        {
            _answerImageService = answerImageService;
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            AnswerImageResponse response = await _answerImageService.GetAsync(UserId, id);

            return Ok(ApiResult<AnswerImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            AnswerImageResponse response = await _answerImageService.GetAsync(id);

            return Ok(ApiResult<AnswerImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("answer/{id}")]
        public async Task<IActionResult> GetByAnswerId(Guid id)
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByAnswerIdAsync(UserId, id);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("answer/{id}/admin")]
        public async Task<IActionResult> GetByAdminAnswerId(Guid id)
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByAnswerIdAsync(id);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("topic/{id}")]
        public async Task<IActionResult> GetByTopicId(Guid id)
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByTopicIdAsync(UserId, id);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("topic/{id}/admin")]
        public async Task<IActionResult> GetByAdminTopicId(Guid id)
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByTopicIdAsync(id);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<AnswerImageResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user/{id}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid id)
        {
            List<AnswerImageResponse> response = await _answerImageService.GetByUserIdAsync(id);

            return Ok(ApiResult<List<AnswerImageResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(AnswerImageRequest request)
        {
            AnswerImageResponse response = await _answerImageService.CreateAsync(UserId, request);

            return Ok(ApiResult<AnswerImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            AnswerImageResponse response = await _answerImageService.DeleteAsync(UserId, id);

            return Ok(ApiResult<AnswerImageResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<AnswerImageResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            AnswerImageResponse response = await _answerImageService.DeleteAsync(id);

            return Ok(ApiResult<AnswerImageResponse>.OK(response));
        }
    }
}
