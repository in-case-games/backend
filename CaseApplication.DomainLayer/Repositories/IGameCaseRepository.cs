using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<GameCase> GetCurrentCase(Guid id);
        public Task<IEnumerable<GameCase>> GetAllCases();
        public Task<bool> CreateCase(GameCase gameCase);
        public Task<bool> UpdateCase(GameCase gameCase);
        public Task<bool> DeleteCase(Guid id);
    }
}
