using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaseInventoryController : ControllerBase
    {
        private readonly ICaseInventoryRepository _caseInventoryRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public CaseInventoryController(ICaseInventoryRepository caseInventoryRepository)
        {
            _caseInventoryRepository = caseInventoryRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            CaseInventory? caseInventory = await _caseInventoryRepository.Get(id);

            if (caseInventory != null)
            {
                return Ok(caseInventory);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("ids/{caseId}&{itemId}")]
        public async Task<IActionResult> GetByIds(Guid caseId, Guid itemId)
        {
            CaseInventory? caseInventory = await _caseInventoryRepository.GetById(caseId, itemId);

            if (caseInventory != null)
            {
                return Ok(caseInventory);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("all/{caseId}")]
        public async Task<IActionResult> GetAll(Guid caseId)
        {
            return Ok(await _caseInventoryRepository.GetAll(caseId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(CaseInventory caseInventory)
        {
            return Ok(await _caseInventoryRepository.Create(caseInventory));
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(CaseInventory caseInventory)
        {
            return Ok(await _caseInventoryRepository.Update(caseInventory));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _caseInventoryRepository.Delete(id));
        }
    }
}
