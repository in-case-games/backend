using Authentication.API.Common;
using Authentication.API.Filters;
using Authentication.BLL;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.API.Controllers
{
    [Route("api/authentication/confirm")]
    [ApiController]
    public class AuthenticationConfirmController : ControllerBase
    {
        private readonly IAuthenticationConfirmService _authConfirmService;

        public AuthenticationConfirmController(IAuthenticationConfirmService authConfirmService)
        {
            _authConfirmService = authConfirmService;
        }

        [ProducesResponseType(typeof(ApiResult<TokensResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("account")]
        public async Task<IActionResult> ConfirmAccount(string token)
        {
            TokensResponse response = await _authConfirmService.ConfirmAccountAsync(token);

            return Ok(ApiResult<TokensResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token)
        {
            UserResponse response = await _authConfirmService.UpdateEmailAsync(email, token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId}/email/{email}")]
        public async Task<IActionResult> UpdateEmail(Guid userId, string email)
        {
            UserResponse response = await _authConfirmService.UpdateEmailByAdminAsync(userId, email);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login, string token)
        {
            UserResponse response = await _authConfirmService.UpdateLoginAsync(login, token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId}/login/{login}")]
        public async Task<IActionResult> UpdateLogin(Guid userId, string login)
        {
            UserResponse response = await _authConfirmService.UpdateLoginByAdminAsync(userId, login);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token)
        {
            UserResponse response = await _authConfirmService.UpdatePasswordAsync(password, token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token)
        {
            UserResponse response = await _authConfirmService.DeleteAsync(token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }
    }
}
