using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameCaseRepository : IBaseRepository<GameCase>
    {
        public Task<IEnumerable<GameCase>> GetAll();
        public Task<IEnumerable<GameCase>> GetAllByGroupName(string name);
    }
}
