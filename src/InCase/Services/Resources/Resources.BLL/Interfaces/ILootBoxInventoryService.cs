using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxInventoryService
    {
        public Task<LootBoxInventoryResponse> GetAsync(Guid id);
        public Task<List<LootBoxInventoryResponse>> GetByBoxIdAsync(Guid id);
        public Task<List<LootBoxInventoryResponse>> GetByItemIdAsync(Guid id);
        public Task<LootBoxInventoryResponse> CreateAsync(LootBoxInventoryRequest request);
        public Task<LootBoxInventoryResponse> UpdateAsync(LootBoxInventoryRequest request);
        public Task<LootBoxInventoryResponse> DeleteAsync(Guid id);
    }
}