using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces;

public interface ILootBoxInventoryService
{
    public Task<LootBoxInventory?> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task CreateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default);
    public Task UpdateAsync(LootBoxInventoryTemplate template, CancellationToken cancellation = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}