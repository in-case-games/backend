using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserInventoryRepository
    {
        public Task<UserInventory> Get(Guid id);
        public Task<IEnumerable<UserInventory>> GetAll(Guid userId);
        public Task<bool> Create(UserInventory userInventory);
        public Task<bool> Update(UserInventory userInventory);
        public Task<bool> Delete(Guid id);
    }
}
