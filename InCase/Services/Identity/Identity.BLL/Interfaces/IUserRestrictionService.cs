using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserRestrictionService
    {
        public Task<UserRestrictionResponse> Get(Guid id);
        public Task<List<UserRestrictionResponse>> GetByUserId(Guid userId);
        public Task<List<UserRestrictionResponse>> Get(Guid userId, Guid ownerId);
        public Task<List<UserRestrictionResponse>> GetByLogin(string login);
        public Task<List<RestrictionTypeResponse>> GetTypes();
        public Task<UserRestrictionResponse> Create(UserRestrictionRequest request);
        public Task<UserRestrictionResponse> Update(UserRestrictionRequest request);
        public Task<UserRestrictionResponse> Delete(Guid id);
    }
}
