using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRestrictionRepository
    {
        public Task<UserRestriction> Get(Guid id);
        public Task<IEnumerable<UserRestriction>> GetAll(Guid userId);
        public Task<bool> Create(UserRestriction userRestriction);
        public Task<bool> Update(UserRestriction userRestriction);
        public Task<bool> Delete(Guid id);
    }
}
