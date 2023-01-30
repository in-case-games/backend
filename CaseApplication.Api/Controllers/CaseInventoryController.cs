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
        public async Task<CaseInventory> Get(Guid id)
        {
            return await _caseInventoryRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<CaseInventory>> GetAll(Guid caseId)
        {
            return await _caseInventoryRepository.GetAll(caseId);
        }

        [HttpPost]
        public async Task<bool> Create(CaseInventory caseInventory)
        {
            return await _caseInventoryRepository.Create(caseInventory);
        }

        [HttpPut]
        public async Task<bool> Update(CaseInventory caseInventory)
        {
            return await _caseInventoryRepository.Update(caseInventory);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _caseInventoryRepository.Delete(id);
        }
    }
}
