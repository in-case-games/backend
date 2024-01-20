using Identity.API.Common;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.API.Controllers
{
    [Route("api/user-role")]
    [ApiController]
    public class UserRoleController(IUserRoleService roleService) : ControllerBase
    {
        [ProducesResponseType(typeof(ApiResult<UserRoleResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellation)
        {
            var response = await roleService.GetAsync(id, cancellation);

            return Ok(ApiResult<UserRoleResponse>.Ok(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<UserRoleResponse>>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var response = await roleService.GetAsync(cancellation);

            return Ok(ApiResult<List<UserRoleResponse>>.Ok(response));
        }
    }
}
