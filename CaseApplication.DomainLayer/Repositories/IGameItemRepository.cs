using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameItemRepository : IBaseRepository<GameItem>
    {
        public Task<GameItem?> GetByName(string name);
        public Task<List<GameItem>> GetAll();
    }
}
