using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository : IBaseRepository<CaseInventory>
    {
        public Task<IEnumerable<CaseInventory>> GetAll(Guid caseId);
    }
}
