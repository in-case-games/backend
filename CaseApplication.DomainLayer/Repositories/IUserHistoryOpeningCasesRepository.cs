using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserHistoryOpeningCasesRepository
    {
        public Task<UserHistoryOpeningCases> Get(Guid id);
        public Task<IEnumerable<UserHistoryOpeningCases>> GetAll(Guid userId);
        public Task<bool> Create(UserHistoryOpeningCases userHistory);
        public Task<bool> Update(UserHistoryOpeningCases userHistory);
        public Task<bool> Delete(Guid id);
        public Task<bool> DeleteAll(Guid userId);
    }
}
