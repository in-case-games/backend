using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameItemRepository
    {
        public Task<GameItem> Get(Guid id);
        public Task<IEnumerable<GameItem>> GetAll();
        public Task<bool> Create(GameItem item);
        public Task<bool> Update(GameItem item);
        public Task<bool> Delete(Guid id);
    }
}
