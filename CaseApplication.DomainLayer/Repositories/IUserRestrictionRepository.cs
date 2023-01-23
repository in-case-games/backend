using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRestrictionRepository
    {
        public Task<UserRestriction> GetRestriction(Guid id);
        public Task<IEnumerable<UserRestriction>> GetAllRestrictions(Guid userId);
        public Task<bool> CreateRestriction(UserRestriction userRestriction);
        public Task<bool> UpdateRestriction(UserRestriction userRestriction);
        public Task<bool> DeleteRestriction(Guid id);
    }
}
