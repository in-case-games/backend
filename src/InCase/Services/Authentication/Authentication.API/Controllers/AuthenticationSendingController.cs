using Authentication.API.Common;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.API.Controllers
{
    [Route("api/authentication/sending")]
    [ApiController]
    public class AuthenticationSendingController : ControllerBase
    {
        private readonly IAuthenticationSendingService _authSendingService;

        public AuthenticationSendingController(IAuthenticationSendingService authSendingService)
        {
            _authSendingService = authSendingService;
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("forgot/password/{login}")]
        public async Task<IActionResult> ForgotPassword(string login)
        {
            await _authSendingService.ForgotPasswordAsync(login);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("email/{login}&{password}")]
        public async Task<IActionResult> UpdateEmail(string login, string password)
        {
            await _authSendingService.UpdateEmailAsync(login, password);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("login/{login}&{password}")]
        public async Task<IActionResult> UpdateLogin(string login, string password)
        {
            await _authSendingService.UpdateLoginAsync(login, password);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("password/{login}&{password}")]
        public async Task<IActionResult> UpdatePassword(string login, string password)
        {
            await _authSendingService.UpdatePasswordAsync(login, password);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("confirm/{login}&{password}")]
        public async Task<IActionResult> DeleteAccount(string login, string password)
        {
            await _authSendingService.DeleteAccountAsync(login, password);

            return Ok(ApiResult<string>.SentEmail());
        }
    }
}
