using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaseInventoryController : ControllerBase
    {
        private readonly ICaseInventoryRepository _caseInventoryRepository;

        public CaseInventoryController(ICaseInventoryRepository caseInventoryRepository)
        {
            _caseInventoryRepository = caseInventoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            CaseInventory? caseInventory = await _caseInventoryRepository.Get(id);

            if (caseInventory != null)
            {
                return Ok(caseInventory);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid caseId)
        {
            return Ok(await _caseInventoryRepository.GetAll(caseId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CaseInventory caseInventory)
        {
            return Ok(await _caseInventoryRepository.Create(caseInventory));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CaseInventory caseInventory)
        {
            return Ok(await _caseInventoryRepository.Update(caseInventory));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _caseInventoryRepository.Delete(id));
        }
    }
}
