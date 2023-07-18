using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces
{
    public interface ILootBoxInventoryService
    {
        public Task CreateAsync(LootBoxInventoryTemplate template);
        public Task UpdateAsync(LootBoxInventoryTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
