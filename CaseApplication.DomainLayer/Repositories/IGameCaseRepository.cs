using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<GameCase> Get(Guid id);
        public Task<IEnumerable<GameCase>> GetAll();
        public Task<IEnumerable<GameCase>> GetAllByGroupName(string name);
        public Task<GameItem> OpenCase(Guid userId, Guid caseId);
        public Task<bool> Create(GameCase gameCase);
        public Task<bool> Update(GameCase gameCase);
        public Task<bool> Delete(Guid id);
    }
}
