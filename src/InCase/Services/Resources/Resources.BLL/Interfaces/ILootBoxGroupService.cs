using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxGroupService
    {
        public Task<LootBoxGroupResponse> GetAsync(Guid id);
        public Task<List<LootBoxGroupResponse>> GetAsync();
        public Task<List<LootBoxGroupResponse>> GetByGameIdAsync(Guid id);
        public Task<List<LootBoxGroupResponse>> GetByBoxIdAsync(Guid id);
        public Task<List<LootBoxGroupResponse>> GetByGroupIdAsync(Guid id);
        public Task<LootBoxGroupResponse> CreateAsync(LootBoxGroupRequest request);
        public Task<LootBoxGroupResponse> UpdateAsync(LootBoxGroupRequest request);
        public Task<LootBoxGroupResponse> DeleteAsync(Guid id);
    }
}
