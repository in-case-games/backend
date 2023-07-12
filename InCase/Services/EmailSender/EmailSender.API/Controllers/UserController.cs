using EmailSender.API.Common;
using EmailSender.API.Filters;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmailSender.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserAdditionalInfoResponse response = await _userService.GetAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AllowAnonymous]
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            UserAdditionalInfoResponse response = await _userService.GetByUserIdAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("info")]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfoResponse response = await _userService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{isNotify}/email")]
        public async Task<IActionResult> ChangeNotifyEmail(bool isNotify)
        {
            UserAdditionalInfoResponse response = await _userService.UpdateNotifyEmailAsync(UserId, isNotify);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}&{isNotify}/email/admin")]
        public async Task<IActionResult> ChangeNotifyEmailByAdmin(Guid id, bool isNotify)
        {
            UserAdditionalInfoResponse response = await _userService.UpdateNotifyEmailAsync(id, isNotify);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }
    }
}
