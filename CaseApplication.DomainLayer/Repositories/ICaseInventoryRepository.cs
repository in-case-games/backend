using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository : IBaseRepository<CaseInventory>
    {
        public Task<CaseInventory?> GetById(Guid caseId, Guid itemId);
        public Task<List<CaseInventory>> GetAll(Guid caseId);
    }
}
