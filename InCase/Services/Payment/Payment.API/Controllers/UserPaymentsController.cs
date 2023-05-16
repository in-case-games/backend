using InCase.Infrastructure.Common;
using InCase.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Payment.BLL.Models;
using Payment.BLL.Services;
using Statistics.API.Common;
using System.Security.Claims;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
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
            List<UserPaymentsResponse> response = await _userPaymentsService.GetAsync(UserId);

            return Ok(ApiResult<List<UserPaymentsResponse>>.OK(response));
        }

        [AuthorizeByRole(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            UserPaymentsResponse response = await _userPaymentsService.GetByIdAsync(id);

            return Ok(ApiResult<UserPaymentsResponse>.OK(response));
        }
    }
}
