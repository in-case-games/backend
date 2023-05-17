using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Security.Claims;

namespace Payment.API.Controllers
{
    [Route("api/user-payments")]
    [ApiController]
    public class UserPaymentsController : ControllerBase
    {
        private readonly IUserPaymentsService _userPaymentsService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserPaymentsController(IUserPaymentsService userPaymentsService)
        {
            _userPaymentsService = userPaymentsService;
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserPaymentsResponse> response = await _userPaymentsService
                .GetAsync(UserId, 100);

            return Ok(ApiResult<List<UserPaymentsResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            UserPaymentsResponse response = await _userPaymentsService
                .GetByIdAsync(id, UserId);

            return Ok(ApiResult<UserPaymentsResponse>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin(int count = 100)
        {
            List<UserPaymentsResponse> response = await _userPaymentsService
                .GetAsync(count);

            return Ok(ApiResult<List<UserPaymentsResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("{userId}/admin")]
        public async Task<IActionResult> GetByUserIdAdmin(Guid userId, int count = 100)
        {
            List<UserPaymentsResponse> response = await _userPaymentsService
                .GetAsync(userId, count);

            return Ok(ApiResult<List<UserPaymentsResponse>>.OK(response));
        }
    }
}
