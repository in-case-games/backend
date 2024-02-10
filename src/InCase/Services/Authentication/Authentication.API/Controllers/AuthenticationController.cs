using Authentication.API.Common;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.API.Controllers;
[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [ProducesResponseType(typeof(ApiResult<string>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(UserRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.SignInAsync(request, cancellationToken);

        return Ok(ApiResult<string>.SentEmail());
    }

    [ProducesResponseType(typeof(ApiResult<string>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(UserRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.SignUpAsync(request, cancellationToken);

        return Ok(ApiResult<string>.SentEmail());
    }

    [ProducesResponseType(typeof(ApiResult<TokensResponse>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshTokens(string token, CancellationToken cancellationToken)
    {
        var response = await authenticationService.RefreshTokensAsync(token, cancellationToken);

        return Ok(ApiResult<TokensResponse>.Ok(response));
    }
}
