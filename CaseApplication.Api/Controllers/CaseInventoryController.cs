using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaseInventoryController : ControllerBase
    {
        public readonly ICaseInventoryRepository _caseInventoryRepository;

        public CaseInventoryController(ICaseInventoryRepository caseInventoryRepository)
        {
            _caseInventoryRepository = caseInventoryRepository;
        }

        [HttpGet]
        public async Task<CaseInventory> GetCaseInventory(Guid id)
        {
            return await _caseInventoryRepository.GetCurrentCaseInventory(id);
        }

        [HttpGet("GetAllCaseInventories")]
        public async Task<IEnumerable<CaseInventory>> GetAllCaseInventories(Guid caseId)
        {
            return await _caseInventoryRepository.GetAllCaseInventory(caseId);
        }

        [HttpPost]
        public async Task<bool> CreateCaseInventory(CaseInventory caseInventory)
        {
            return await _caseInventoryRepository.CaseInventoryCreate(caseInventory);
        }

        [HttpPut]
        public async Task<bool> UpdateCaseInventory(CaseInventory caseInventory)
        {
            return await _caseInventoryRepository.CaseInventoryUpdate(caseInventory);
        }

        [HttpDelete]
        public async Task<bool> DeleteCaseInventory(Guid id)
        {
            return await _caseInventoryRepository.CaseInventoryDelete(id);
        }
    }
}
