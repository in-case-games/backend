﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review.API.Common;
using Review.API.Filters;
using Review.BLL.Interfaces;
using Review.BLL.Models;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ReviewImageResponse> response = await _reviewImageService.GetAsync(isOnlyApproved: true);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
        {
            List<ReviewImageResponse> response = await _reviewImageService.GetAsync(isOnlyApproved: false);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            ReviewImageResponse response = await _reviewImageService.GetAsync(id, isOnlyApproved: true);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            ReviewImageResponse response = await _reviewImageService.GetAsync(id, isOnlyApproved: false);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(userId, isOnlyApproved: true);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId()
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(UserId, isOnlyApproved: false);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin/user/{id}")]
        public async Task<IActionResult> GetByAdminUserId(Guid id)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(id, isOnlyApproved: false);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("review/{id}")]
        public async Task<IActionResult> GetByReviewId(Guid id)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(id, isOnlyApproved: true);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("review/{id}")]
        public async Task<IActionResult> GetByAdminReviewId(Guid id)
        {
            List<ReviewImageResponse> response = await _reviewImageService
                .GetByUserIdAsync(id, isOnlyApproved: false);

            return Ok(ApiResult<List<ReviewImageResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpPost]
        public async Task<IActionResult> Post(ReviewImageRequest request)
        {
            ReviewImageResponse response = await _reviewImageService.CreateAsync(UserId, request);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ReviewImageResponse response = await _reviewImageService.DeleteAsync(UserId, id);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}&{userId}")]
        public async Task<IActionResult> Delete(Guid id, Guid userId)
        {
            ReviewImageResponse response = await _reviewImageService.DeleteAsync(userId, id);

            return Ok(ApiResult<ReviewImageResponse>.OK(response));
        }
    }
}
