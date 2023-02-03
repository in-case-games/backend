using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IUserRestrictionRepository _userRestrictionRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IUserRestrictionRepository userRestrictionRepository)
        {
            _userRestrictionRepository = userRestrictionRepository;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userRestrictionRepository.GetAll(UserId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestriction userRestriction)
        {
            return Ok(await _userRestrictionRepository.Create(userRestriction));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestriction userRestriction)
        {
            return Ok(await _userRestrictionRepository.Update(userRestriction));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userRestrictionRepository.Delete(id));
        }
    }
}
