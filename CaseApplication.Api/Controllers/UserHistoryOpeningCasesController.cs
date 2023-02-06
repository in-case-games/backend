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
        [HttpGet]
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
        [HttpGet("GetAllById")]
        public async Task<IActionResult> GetAllById(Guid userId)
        {
            return Ok(await _userHistoryRepository.GetAllById(userId));
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userHistoryRepository.GetAll());
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userHistoryRepository.Delete(id));
        }

        [Authorize]
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAll(Guid userId)
        {
            return Ok(await _userHistoryRepository.DeleteAll(userId));
        }
    }
}
