using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxService
    {
        public Task<LootBoxResponse> GetAsync(Guid id, CancellationToken cancellation = default);
        public Task<LootBoxResponse> GetAsync(string name, CancellationToken cancellation = default);
        public Task<List<LootBoxResponse>> GetAsync(CancellationToken cancellation = default);
        public Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default);
        public Task<LootBoxResponse> CreateAsync(LootBoxRequest request, CancellationToken cancellation = default);
        public Task<LootBoxResponse> UpdateAsync(LootBoxRequest request, CancellationToken cancellation = default);
        public Task<LootBoxResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
