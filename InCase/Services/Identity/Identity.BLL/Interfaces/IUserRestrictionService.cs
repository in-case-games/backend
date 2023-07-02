using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserRestrictionService
    {
        public Task<UserRestrictionResponse> GetAsync(Guid id);
        public Task<List<UserRestrictionResponse>> GetByUserIdAsync(Guid userId);
        public Task<List<UserRestrictionResponse>> GetByOwnerIdAsync(Guid ownerId);
        public Task<List<UserRestrictionResponse>> GetAsync(Guid userId, Guid ownerId);
        public Task<List<UserRestrictionResponse>> GetByLoginAsync(string login);
        public Task<List<RestrictionTypeResponse>> GetTypesAsync();
        public Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request);
        public Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request);
        public Task<UserRestrictionResponse> DeleteAsync(Guid id);

        public Task DoWorkManagerAsync(CancellationToken cancellationToken);
    }
}
