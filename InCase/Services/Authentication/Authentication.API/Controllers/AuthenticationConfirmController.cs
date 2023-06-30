using Authentication.API.Common;
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
            TokensResponse response = await _authConfirmService.ConfirmAccount(token);

            return Ok(ApiResult<TokensResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> UpdateEmail(string email, string token)
        {
            UserResponse response = await _authConfirmService.UpdateEmail(email, token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("password/{password}")]
        public async Task<IActionResult> UpdatePassword(string password, string token)
        {
            UserResponse response = await _authConfirmService.UpdatePassword(password, token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<UserResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("account")]
        public async Task<IActionResult> Delete(string token)
        {
            UserResponse response = await _authConfirmService.Delete(token);

            return Ok(ApiResult<UserResponse>.OK(response));
        }
    }
}
