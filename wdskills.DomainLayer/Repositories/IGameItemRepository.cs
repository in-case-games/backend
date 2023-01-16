using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IGameItemRepository
    {
        public Task<bool> CreateItem(GameItem item);
        public Task<bool> UpdateItem(GameItem item, Guid id);
        public Task<bool> DeleteItem(Guid id);
        public Task<GameItem> GetCurrentItem(Guid id);
        public Task<IEnumerable<GameItem>> GetItems();
    }
}
