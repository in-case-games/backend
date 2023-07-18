using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces
{
    public interface ILootBoxService
    {
        public Task CreateAsync(LootBoxTemplate template);
        public Task UpdateAsync(LootBoxTemplate template);
        public Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
