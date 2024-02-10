using Resources.BLL.Models;

namespace Resources.BLL.Interfaces;
public interface ILootBoxInventoryService
{
    public Task<LootBoxInventoryResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<LootBoxInventoryResponse>> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<LootBoxInventoryResponse>> GetByItemIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<LootBoxInventoryResponse> CreateAsync(LootBoxInventoryRequest request, CancellationToken cancellation = default);
    public Task<LootBoxInventoryResponse> UpdateAsync(LootBoxInventoryRequest request, CancellationToken cancellation = default);
    public Task<LootBoxInventoryResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}