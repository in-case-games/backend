using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<bool> CreateCase(GameCase gameCase);
        public Task<bool> AddItem(Guid id, GameItem item);
        public Task<bool> RemoveItem(Guid id, GameItem item);
        public Task<bool> DeleteCase(Guid id);
        public Task<bool> UpdateCase(Guid id, GameCase gameCase);
        public Task<bool> GetCurrentCase(Guid id);
        public Task<GameCase> GetAllCases();

    }
}
