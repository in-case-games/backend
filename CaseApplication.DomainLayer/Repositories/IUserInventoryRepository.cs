using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserInventoryRepository
    {
        public Task<UserInventory?> Get(Guid id);
        public Task<List<UserInventory>> GetAll(Guid userId);
        public Task<bool> Create(UserInventoryDto inventoryDto);
        public Task<bool> Update(UserInventoryDto inventoryDto);
        public Task<bool> Delete(Guid id);
    }
}
