using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserHistoryOpeningCasesController : ControllerBase
    {
        private readonly IUserHistoryOpeningCasesRepository _userHistoryRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserHistoryOpeningCasesController(IUserHistoryOpeningCasesRepository userHistoryRepository)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserHistoryOpeningCases? userHistoryOpening = await _userHistoryRepository.Get(id);

            if(userHistoryOpening != null)
            {
                return Ok(userHistoryOpening);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            return Ok(await _userHistoryRepository.GetAllById(userId));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userHistoryRepository.GetAll());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserHistoryOpeningCases? userHistoryOpening = await _userHistoryRepository.Get(id);

            if (!userHistoryOpening!.UserId.Equals(UserId))
                return Forbid("Invalid operation");
            return Ok(await _userHistoryRepository.Delete(id));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            return Ok(await _userHistoryRepository.DeleteAll(UserId));
        }
    }
}
