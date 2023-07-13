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
    [Route("api/support-topic")]
    [ApiController]
    public class SupportTopicController : ControllerBase
    {
        private readonly ISupportTopicService _topicService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public SupportTopicController(ISupportTopicService topicService)
        {
            _topicService = topicService;
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            SupportTopicResponse response = await _topicService.GetAsync(UserId, id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            SupportTopicResponse response = await _topicService.GetAsync(id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            List<SupportTopicResponse> response = await _topicService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{id}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid id)
        {
            List<SupportTopicResponse> response = await _topicService.GetByUserIdAsync(id);

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("opened")]
        public async Task<IActionResult> GetOpenedTopics()
        {
            List<SupportTopicResponse> response = await _topicService.GetOpenedTopicsAsync();

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(Guid id)
        {
            SupportTopicResponse response = await _topicService.CloseTopic(UserId, id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/close/admin")]
        public async Task<IActionResult> CloseByAdmin(Guid id)
        {
            SupportTopicResponse response = await _topicService.CloseTopic(id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(SupportTopicRequest request)
        {
            request.UserId = UserId;

            SupportTopicResponse response = await _topicService.CreateAsync(request);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(SupportTopicRequest request)
        {
            request.UserId = UserId;

            SupportTopicResponse response = await _topicService.UpdateAsync(request);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            SupportTopicResponse response = await _topicService.DeleteAsync(UserId, id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            SupportTopicResponse response = await _topicService.DeleteAsync(id);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }
    }
}
