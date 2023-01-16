using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<bool> CreateCase(GameCase gameCase);
        public Task<bool> AddItem(GameItem item);
        public Task<bool> RemoveItem(GameItem item);
        public Task<bool> DeleteCase(GameCase gameCase);
        public Task<bool> UpdateCase(GameCase gameCase);
        public Task<bool> GetCurrentCase(Guid id);
        public Task<GameCase> GetAllCases();

    }
}
