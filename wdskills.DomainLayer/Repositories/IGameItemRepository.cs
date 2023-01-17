using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameItemRepository
    {
        public Task<bool> CreateItem(GameItem item);
        public Task<bool> UpdateItem(GameItem item);
        public Task<bool> DeleteItem(Guid id);
        public Task<GameItem> GetCurrentItem(Guid id);
        public Task<IEnumerable<GameItem>> GetItems();
    }
}
