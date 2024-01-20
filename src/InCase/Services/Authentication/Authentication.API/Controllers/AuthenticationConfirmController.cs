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
    public class AuthenticationConfirmController(IAuthenticationConfirmService authConfirmService) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResult<TokensResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("account")]
        public async Task<IActionResult> ConfirmAccount(string token, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.ConfirmAccountAsync(token, cancellationToken);

            return Ok(ApiResult<TokensResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.UpdateEmailAsync(email, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId:guid}/email/{email}")]
        public async Task<IActionResult> UpdateEmail(Guid userId, string email, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.UpdateEmailByAdminAsync(userId, email, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login, string token, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.UpdateLoginAsync(login, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.Owner)]
        [HttpGet("{userId:guid}/login/{login}")]
        public async Task<IActionResult> UpdateLogin(Guid userId, string login, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.UpdateLoginByAdminAsync(userId, login, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.UpdatePasswordAsync(password, token, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token, CancellationToken cancellationToken)
        {
            var response = await authConfirmService.DeleteAsync(token, cancellationToken);

            return Ok(ApiResult<UserResponse>.Ok(response));
        }
    }
}
