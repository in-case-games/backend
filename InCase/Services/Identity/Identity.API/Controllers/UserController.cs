using Identity.API.Common;
using Identity.API.Filters;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Identity.API.Controllers
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

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserResponse response = await _userService.Get(id);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            UserResponse response = await _userService.Get(UserId);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> Get(string login)
        {
            UserResponse response = await _userService.Get(login);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpPut]
        public async Task<IActionResult> Get(UserRequest request)
        {
            request.Id = UserId;

            UserResponse response = await _userService.UpdateLogin(request);

            return Ok(ApiResult<UserResponse>.OK(response));
        }
    }
}
