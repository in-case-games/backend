using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameCaseRepository : IBaseRepository<GameCase>
    {
        public Task<List<GameCase>> GetAll();
        public Task<List<GameCase>> GetAllByGroupName(string name);
    }
}
