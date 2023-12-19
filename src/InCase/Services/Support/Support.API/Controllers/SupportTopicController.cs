﻿using Microsoft.AspNetCore.Mvc;
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
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public SupportTopicController(ISupportTopicService topicService)
        {
            _topicService = topicService;
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.GetAsync(UserId, id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.GetAsync(id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
        {
            var response = await _topicService.GetByUserIdAsync(UserId, cancellation);

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await _topicService.GetByUserIdAsync(userId, cancellation);

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<SupportTopicResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("opened")]
        public async Task<IActionResult> GetOpenedTopics(CancellationToken cancellation)
        {
            var response = await _topicService.GetOpenedTopicsAsync(cancellation);

            return Ok(ApiResult<List<SupportTopicResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.CloseTopic(UserId, id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/close/admin")]
        public async Task<IActionResult> CloseByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.CloseTopic(id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(SupportTopicRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;

            var response = await _topicService.CreateAsync(request, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(SupportTopicRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;

            var response = await _topicService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<SupportTopicResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _topicService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<SupportTopicResponse>.OK(response));
        }
    }
}
