using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<CaseInventory?> Get(Guid id);
        public Task<CaseInventory?> GetById(Guid caseId, Guid itemId);
        public Task<List<CaseInventory>> GetAll(Guid caseId);
        public Task<bool> Create(CaseInventoryDto caseInventoryDto);
        public Task<bool> Update(CaseInventoryDto caseInventoryDto);
        public Task<bool> Delete(Guid id);
    }
}
