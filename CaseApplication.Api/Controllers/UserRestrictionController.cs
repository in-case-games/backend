using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IUserRestrictionRepository _userRestrictionRepository;

        public UserRestrictionController(IUserRestrictionRepository userRestrictionRepository)
        {
            _userRestrictionRepository = userRestrictionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            UserRestriction? userRestriction = await _userRestrictionRepository.Get(id);

            if (userRestriction != null)
            {
                return Ok(userRestriction);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            return Ok(await _userRestrictionRepository.GetAll(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRestriction userRestriction)
        {
            return Ok(await _userRestrictionRepository.Create(userRestriction));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserRestriction userRestriction)
        {
            return Ok(await _userRestrictionRepository.Update(userRestriction));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userRestrictionRepository.Delete(id));
        }
    }
}
