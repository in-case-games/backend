using Resources.BLL.Models;

namespace Resources.BLL.Interfaces;

public interface ILootBoxBannerService
{
    public Task<LootBoxBannerResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<LootBoxBannerResponse>> GetAsync(CancellationToken cancellation = default);
    public Task<List<LootBoxBannerResponse>> GetAsync(bool isActive, CancellationToken cancellation = default);
    public Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default);
    public Task<LootBoxBannerResponse> UpdateAsync(LootBoxBannerRequest request, CancellationToken cancellation = default);
    public Task<LootBoxBannerResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}