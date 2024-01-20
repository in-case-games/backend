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
    public class SupportTopicAnswerController(ISupportTopicAnswerService answerService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await answerService.GetAsync(UserId, id, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id:guid}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await answerService.GetAsync(id, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await answerService.GetByUserIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId:guid}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await answerService.GetByUserIdAsync(userId, cancellation);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("topic/{id:guid}")]
        public async Task<IActionResult> GetByTopicId(Guid id, CancellationToken cancellation)
        {
            var response = await answerService.GetByTopicIdAsync(UserId, id, cancellation);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicAnswerResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("topic/{id:guid}/admin")]
        public async Task<IActionResult> GetByAdminTopicId(Guid id, CancellationToken cancellation)
        {
            var response = await answerService.GetByTopicIdAsync(id, cancellation);

            return Ok(ApiResult<List<SupportTopicAnswerResponse>>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(SupportTopicAnswerRequest request, CancellationToken cancellation)
        {
            request.PlaintiffId = UserId;

            var response = await answerService.CreateAsync(request, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPost("admin")]
        public async Task<IActionResult> PostByAdmin(SupportTopicAnswerRequest request, CancellationToken cancellation)
        {
            request.PlaintiffId = UserId;

            var response = await answerService.CreateByAdminAsync(request, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(SupportTopicAnswerRequest request, CancellationToken cancellation)
        {
            request.PlaintiffId = UserId;

            var response = await answerService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicAnswerResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await answerService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<SupportTopicAnswerResponse>.Ok(response));
        }
    }
}
