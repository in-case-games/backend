using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<bool> CaseInventoryUpdate(CaseInventory caseInventory);
    }
}
