using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<CaseInventory> Get(Guid id);
        public Task<IEnumerable<CaseInventory>> GetAll(Guid caseId);
        public Task<bool> Create(CaseInventory caseInventory);
        public Task<bool> Update(CaseInventory caseInventory);
        public Task<bool> Delete(Guid id);
    }
}
