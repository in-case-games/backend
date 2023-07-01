using Microsoft.AspNetCore.Mvc;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.API.Common;
using Identity.API.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : Controller
    {
        private readonly IUserAdditionalInfoService _userInfoService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserAdditionalInfoResponse response = await _userInfoService.GetAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            UserAdditionalInfoResponse response = await _userInfoService.GetByUserIdAsync(userId);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfoResponse response = await _userInfoService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner)]
        [HttpPut("role")]
        public async Task<IActionResult> UpdateRole(UserAdditionalInfoRequest request)
        {
            UserAdditionalInfoResponse response = await _userInfoService.UpdateRole(request);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpPut("deletion/date")]
        public async Task<IActionResult> UpdateDeletionDate(UserAdditionalInfoRequest request)
        {
            UserAdditionalInfoResponse response = await _userInfoService.UpdateDeletionDate(request);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(UserAdditionalInfoRequest request)
        {
            UserAdditionalInfoResponse response = await _userInfoService.UpdateImage(request);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }
    }
}