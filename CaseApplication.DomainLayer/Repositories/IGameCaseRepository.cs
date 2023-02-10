using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IGameCaseRepository
    {
        public Task<GameCase?> Get(Guid id);
        public Task<GameCase?> GetByName(string name);
        public Task<List<GameCase>> GetAll();
        public Task<List<GameCase>> GetAllByGroupName(string name);
        public Task<bool> Create(GameCaseDto gameCaseDto);
        public Task<bool> Update(GameCase gameCase);
        public Task<bool> Delete(Guid id);
    }
}
