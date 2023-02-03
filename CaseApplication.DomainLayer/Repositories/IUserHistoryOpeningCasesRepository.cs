using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserHistoryOpeningCasesRepository 
        : IBaseRepository<UserHistoryOpeningCases>
    {
        public Task<List<UserHistoryOpeningCases>> GetAll();
        public Task<List<UserHistoryOpeningCases>> GetAllById(Guid userId);
        public Task<bool> DeleteAll(Guid userId);
    }
}
