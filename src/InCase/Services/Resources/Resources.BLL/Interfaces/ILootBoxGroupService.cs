using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxGroupService
    {
        public Task<LootBoxGroupResponse> GetAsync(Guid id, CancellationToken cancellation = default);
        public Task<List<LootBoxGroupResponse>> GetAsync(CancellationToken cancellation = default);
        public Task<List<LootBoxGroupResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default);
        public Task<List<LootBoxGroupResponse>> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default);
        public Task<List<LootBoxGroupResponse>> GetByGroupIdAsync(Guid id, CancellationToken cancellation = default);
        public Task<LootBoxGroupResponse> CreateAsync(LootBoxGroupRequest request, CancellationToken cancellation = default);
        public Task<LootBoxGroupResponse> UpdateAsync(LootBoxGroupRequest request, CancellationToken cancellation = default);
        public Task<LootBoxGroupResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
