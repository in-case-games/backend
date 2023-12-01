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
        public async Task<IActionResult> ConfirmAccount(string token, CancellationToken cancellationToken)
        {
            TokensResponse response = await _authConfirmService.ConfirmAccountAsync(token, cancellationToken);

            return Ok(ApiResult<TokensResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.UpdateEmailAsync(email, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId}/email/{email}")]
        public async Task<IActionResult> UpdateEmail(Guid userId, string email, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.UpdateEmailByAdminAsync(userId, email, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login, string token, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.UpdateLoginAsync(login, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId}/login/{login}")]
        public async Task<IActionResult> UpdateLogin(Guid userId, string login, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.UpdateLoginByAdminAsync(userId, login, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.UpdatePasswordAsync(password, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token, CancellationToken cancellationToken)
        {
            UserResponse response = await _authConfirmService.DeleteAsync(token, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(response));
        }
    }
}
