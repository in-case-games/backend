using Authentication.API.Common;
using Authentication.BLL;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(UserRequest request, CancellationToken cancellationToken)
        {
            await _authenticationService.SignInAsync(request, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserRequest request, CancellationToken cancellationToken)
        {
            await _authenticationService.SignUpAsync(request, cancellationToken);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<TokensResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string token, CancellationToken cancellationToken)
        {
            TokensResponse response = await _authenticationService.RefreshTokensAsync(token, cancellationToken);

            return Ok(ApiResult<TokensResponse>.OK(response));
        }
    }
}
