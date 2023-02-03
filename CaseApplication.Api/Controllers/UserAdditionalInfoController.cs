using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IUserAdditionalInfoRepository _userInfoRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IUserAdditionalInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            UserAdditionalInfo? userAdditionalInfo = await _userInfoRepository.Get(UserId);

            if (userAdditionalInfo != null) {
                return Ok(userAdditionalInfo);
            }

            return NotFound();
        }
    }
}
