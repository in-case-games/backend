using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IUserRestrictionService
    {
        public Task<UserRestrictionResponse> GetAsync(Guid id);
        public Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request, bool isNewGuid = false);
        public Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request);
        public Task<UserRestrictionResponse> DeleteAsync(Guid id);
    }
}
