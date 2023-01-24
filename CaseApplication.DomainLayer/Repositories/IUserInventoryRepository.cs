using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserInventoryRepository
    {
        public Task<UserInventory> GetUserInventory(Guid id);
        public Task<IEnumerable<UserInventory>> GetAllUserInventories(Guid userId);
        public Task<bool> CreateUserInventory(UserInventory userInventory);
        public Task<bool> UpdateUserInventory(UserInventory userInventory);
        public Task<bool> DeleteUserInventory(Guid id);
    }
}
