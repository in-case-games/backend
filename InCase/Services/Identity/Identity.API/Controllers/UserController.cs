using Microsoft.AspNetCore.Mvc;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.API.Common;
using Identity.API.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        
        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            UserResponse user = await _userService.GetAsync(UserId, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(user));
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            UserResponse user = await _userService.GetAsync(id, cancellationToken);

            return Ok(ApiResult<UserResponse>.OK(user));
        }
        [AllowAnonymous]
        [HttpGet("{range}")]
        public async Task<IActionResult> Get(int range = 100, CancellationToken cancellationToken = default)
        {
            List<UserResponse> users = await _userService.GetAsync(range, cancellationToken);

            return Ok(ApiResult<List<UserResponse>>.OK(users));
        }
        [AllowAnonymous]
        [HttpGet("{roles}")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken = default)
        {
            List<UserRoleResponse> roles = await _userService.GetRolesAsync(cancellationToken);

            return Ok(ApiResult<List<UserRoleResponse>>.OK(roles));
        }
        [AuthorizeByRole(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoRequest info, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateAsync(info, cancellationToken);

            return Ok(ApiResult<UserAdditionalInfoRequest>.OK(info));
        }
    }
}