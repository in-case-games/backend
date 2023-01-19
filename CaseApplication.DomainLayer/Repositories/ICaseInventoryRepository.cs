using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<CaseInventory> GetCurrentCaseInventory(Guid id);
        public Task<IEnumerable<CaseInventory>> GetAllCaseInventory(Guid caseId);
        public Task<bool> CaseInventoryCreate(CaseInventory caseInventory);
        public Task<bool> CaseInventoryUpdate(CaseInventory caseInventory);
        public Task<bool> CaseInventoryDelete(Guid id);
    }
}
