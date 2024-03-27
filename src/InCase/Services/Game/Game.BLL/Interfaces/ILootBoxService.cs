using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces;
public interface ILootBoxService
{
    public Task<LootBox?> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task CreateAsync(LootBoxTemplate template, CancellationToken cancellation = default);
    public Task UpdateAsync(LootBoxTemplate template, CancellationToken cancellation = default);
    public Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template, CancellationToken cancellation = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}