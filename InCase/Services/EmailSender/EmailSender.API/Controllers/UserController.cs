using EmailSender.API.Common;
using EmailSender.API.Filters;
using EmailSender.BLL.Interfaces;
using EmailSender.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace EmailSender.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAdditionalInfoService _userService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(IUserAdditionalInfoService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}/is-notify")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            UserAdditionalInfoResponse response = await _userService.GetByUserIdAsync(id);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("is-notify")]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfoResponse response = await _userService.GetByUserIdAsync(UserId);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("is-notify/{isNotify}")]
        public async Task<IActionResult> ChangeNotifyEmail(bool isNotify)
        {
            UserAdditionalInfoResponse response = await _userService.UpdateNotifyEmailAsync(UserId, isNotify);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserAdditionalInfoResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/is-notify/{isNotify}/admin")]
        public async Task<IActionResult> ChangeNotifyEmailByAdmin(Guid id, bool isNotify)
        {
            UserAdditionalInfoResponse response = await _userService.UpdateNotifyEmailAsync(id, isNotify);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }
    }
}
