using Microsoft.AspNetCore.Mvc;
using Support.API.Common;
using Support.API.Filters;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using System.Net;
using System.Security.Claims;

namespace Support.API.Controllers
{
    [Route("api/support-topic-answer")]
    [ApiController]
    public class SupportTopicAnswerController : ControllerBase
    {
        private readonly ISupportTopicAnswerService _answerService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public SupportTopicAnswerController(ISupportTopicAnswerService answerService)
        {
            _answerService = answerService;
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            SupportTopicAnswerResponse response = await _answerService.GetAsync(UserId, id);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            SupportTopicAnswerResponse response = await _answerService.GetAsync(id);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<SupportTopicAnswerResponse> response = await _answerService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid id)
        {
            List<SupportTopicAnswerResponse> response = await _answerService.GetByUserIdAsync(id);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("topic/{id}")]
        public async Task<IActionResult> GetByTopicId(Guid id)
        {
            List<SupportTopicAnswerResponse> response = await _answerService.GetByTopicIdAsync(UserId, id);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("topic/{id}/admin")]
        public async Task<IActionResult> GetByAdminTopicId(Guid id)
        {
            List<SupportTopicAnswerResponse> response = await _answerService.GetByTopicIdAsync(id);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(SupportTopicAnswerRequest request)
        {
            request.PlaintiffId = UserId;

            SupportTopicAnswerResponse response = await _answerService.CreateAsync(request);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPost("admin")]
        public async Task<IActionResult> PostByAdmin(SupportTopicAnswerRequest request)
        {
            request.PlaintiffId = UserId;

            SupportTopicAnswerResponse response = await _answerService.CreateByAdminAsync(request);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(SupportTopicAnswerRequest request)
        {
            request.PlaintiffId = UserId;

            SupportTopicAnswerResponse response = await _answerService.UpdateAsync(request);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            SupportTopicAnswerResponse response = await _answerService.DeleteAsync(UserId, id);

            return Ok(ApiResult<SupportTopicAnswerResponse>.OK(response));
        }
    }
}
