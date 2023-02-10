using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserHistoryOpeningCasesRepository
    {
        public Task<UserHistoryOpeningCases?> Get(Guid id); 
        public Task<List<UserHistoryOpeningCases>> GetAll();
        public Task<List<UserHistoryOpeningCases>> GetAllById(Guid userId);
        public Task<bool> Create(UserHistoryOpeningCasesDto historyDto);
        public Task<bool> Delete(Guid id);
        public Task<bool> DeleteAll(Guid userId);
    }
}
