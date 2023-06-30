using Microsoft.AspNetCore.Mvc;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.API.Common;
using Identity.API.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : Controller
    {
        private readonly IUserAdditionalInfoService _userInfoService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            UserAdditionalInfoResponse response = await _userInfoService.GetAsync(id, cancellationToken);

            return Ok(ApiResult<UserAdditionalInfoResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoRequest info, CancellationToken cancellationToken = default)
        {
            await _userInfoService.UpdateAsync(info, cancellationToken);

            return Ok(ApiResult<UserAdditionalInfoRequest>.OK(info));
        }
    }
}