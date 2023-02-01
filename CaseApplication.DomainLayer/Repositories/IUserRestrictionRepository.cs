using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRestrictionRepository : IBaseRepository<UserRestriction>
    {
        public Task<List<UserRestriction>> GetAll(Guid userId);
    }
}
