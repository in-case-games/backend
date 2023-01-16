using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<bool> CreateCase(GameCase gameCase);
        public Task<bool> AddItem(Guid id, GameItem item);
        public Task<bool> RemoveItem(Guid id);
        public Task<bool> DeleteCase(Guid id);
        public Task<bool> UpdateCase(GameCase gameCase);
        public Task<GameCase> GetCurrentCase(Guid id);
        public Task<IEnumerable<GameCase>> GetAllCases();

    }
}
