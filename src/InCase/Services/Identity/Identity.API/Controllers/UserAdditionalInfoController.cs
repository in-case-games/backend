using Microsoft.AspNetCore.Mvc;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.API.Common;
using Identity.API.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Identity.API.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : Controller
    {
        private readonly IUserAdditionalInfoService _infoService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoService infoService)
        {
            _infoService = infoService;
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserAdditionalInfoResponse response = await _infoService.GetAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            UserAdditionalInfoResponse response = await _infoService.GetByUserIdAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfoResponse response = await _infoService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("role/{id}&{userId}")]
        public async Task<IActionResult> UpdateRole(Guid id, Guid userId)
        {
            UserAdditionalInfoResponse response = await _infoService.UpdateRoleAsync(userId, id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("deletion/date/{userId}")]
        public async Task<IActionResult> UpdateDeletionDate(Guid userId, DateTime? date = null)
        {
            UserAdditionalInfoResponse response = await _infoService.UpdateDeletionDateAsync(userId, date);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPut("image/{userId}")]
        public async Task<IActionResult> UpdateImage(IFormFile image, [FromForm] Guid userId)
        {
            UserAdditionalInfoResponse response = await _infoService
                .UpdateImageAsync(userId, image);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile image)
        {
            UserAdditionalInfoResponse response = await _infoService.UpdateImageAsync(UserId, image);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }
    }
}