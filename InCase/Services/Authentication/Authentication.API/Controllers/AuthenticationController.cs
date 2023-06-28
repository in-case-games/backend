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
        [HttpGet("sign-in")]
        public async Task<IActionResult> SignIn(UserRequest request)
        {
            await _authenticationService.SignIn(request);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<string>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("sign-up")]
        public async Task<IActionResult> SignUp(UserRequest request)
        {
            await _authenticationService.SignUp(request);

            return Ok(ApiResult<string>.SentEmail());
        }

        [ProducesResponseType(typeof(ApiResult<TokensResponse>),
            (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokens(string token)
        {
            TokensResponse response = await _authenticationService.RefreshTokens(token);

            return Ok(ApiResult<TokensResponse>.OK(response));
        }
    }
}
