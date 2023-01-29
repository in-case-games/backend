using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserHistoryOpeningCasesRepository 
        : IBaseRepository<UserHistoryOpeningCases>
    {
        public Task<IEnumerable<UserHistoryOpeningCases>> GetAll(Guid userId);
        public Task<bool> DeleteAll(Guid userId);
    }
}
