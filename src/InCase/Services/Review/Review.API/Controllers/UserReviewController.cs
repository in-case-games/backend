﻿using Microsoft.AspNetCore.Authorization;
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
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserReviewController(IUserReviewService userReviewService)
        {
            _userReviewService = userReviewService;
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>), 
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await _userReviewService.GetAsync(true, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("last/{count}")]
        public async Task<IActionResult> GetLast(CancellationToken cancellation, int count = 100)
        {
            var response = await _userReviewService.GetAsync(true, count > 100 ? 100 : count, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("last/{count}/admin")]
        public async Task<IActionResult> GetLastAdmin(CancellationToken cancellation, int count = 100)
        {
            var response = await _userReviewService.GetAsync(false, count > 1000 ? 1000 : count, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.GetAsync(id, true, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.GetByUserIdAsync(id, true, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellation)
        {
            var response = await _userReviewService.GetAsync(false, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.GetAsync(id, false, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("user/{userId}/admin")]
        public async Task<IActionResult> GetByAdminUserId(Guid userId, CancellationToken cancellation)
        {
            var response = await _userReviewService.GetByUserIdAsync(userId, false, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserReviewResponse>>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> GetByUserId(CancellationToken cancellation)
        {
            var response = await _userReviewService.GetByUserIdAsync(UserId, false, cancellation);

            return Ok(ApiResult<List<UserReviewResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.ApproveReviewAsync(id, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/denied")]
        public async Task<IActionResult> Denied(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.DeniedReviewAsync(id, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(UserReviewRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;

            var response = await _userReviewService.CreateAsync(request, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Put(UserReviewRequest request, CancellationToken cancellation)
        {
            request.UserId = UserId;

            var response = await _userReviewService.UpdateAsync(request, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.DeleteAsync(UserId, id, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserReviewResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}/admin")]
        public async Task<IActionResult> DeleteAdmin(Guid id, CancellationToken cancellation)
        {
            var response = await _userReviewService.DeleteAsync(id, cancellation);

            return Ok(ApiResult<UserReviewResponse>.OK(response));
        }
    }
}
