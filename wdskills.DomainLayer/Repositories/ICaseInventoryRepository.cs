using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<bool> CaseInventoryUpdate(CaseInventory caseInventory);
    }
}
