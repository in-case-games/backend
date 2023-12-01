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
        [HttpPut("forgot/password")]
        public async Task<IActionResult> ForgotPassword(DataMailRequest request, CancellationToken cancellationToken)
        {
            await _authSendingService.ForgotPasswordAsync(request, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> UpdateEmail(DataMailRequest request, string password, CancellationToken cancellationToken)
        {
            await _authSendingService.UpdateEmailAsync(request, password, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPut("login/{password}")]
        public async Task<IActionResult> UpdateLogin(DataMailRequest request, string password, CancellationToken cancellationToken)
        {
            await _authSendingService.UpdateLoginAsync(request, password, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePassword(DataMailRequest request, string password, CancellationToken cancellationToken)
        {
            await _authSendingService.UpdatePasswordAsync(request, password, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpDelete("account/{password}")]
        public async Task<IActionResult> DeleteAccount(DataMailRequest request, string password, CancellationToken cancellationToken)
        {
            await _authSendingService.DeleteAccountAsync(request, password, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }
    }
}
