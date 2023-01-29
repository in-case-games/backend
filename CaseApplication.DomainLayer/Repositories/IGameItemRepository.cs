using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameItemRepository : IBaseRepository<GameItem>
    {
        public Task<IEnumerable<GameItem>> GetAll();
    }
}
