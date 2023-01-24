using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserHistoryOpeningCasesRepository
    {
        public Task<UserHistoryOpeningCases> GetUserHistory(Guid id);
        public Task<IEnumerable<UserHistoryOpeningCases>> GetAllUserHistories(Guid userId);
        public Task<bool> CreateUserHisory(UserHistoryOpeningCases userHistory);
        public Task<bool> UpdateUserHistory(UserHistoryOpeningCases userHistory);
        public Task<bool> DeleteUserHistory(Guid id);
        public Task<bool> DeleteAllUserHistories(Guid userId);
    }
}
