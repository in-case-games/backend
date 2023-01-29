using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserInventoryRepository : IBaseRepository<UserInventory>
    {
        public Task<IEnumerable<UserInventory>> GetAll(Guid userId);
    }
}
