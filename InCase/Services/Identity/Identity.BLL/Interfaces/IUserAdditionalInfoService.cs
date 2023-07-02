using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id);
        public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId);
        public Task<UserAdditionalInfoResponse> UpdateRoleAsync(UserAdditionalInfoRequest request);
        public Task<UserAdditionalInfoResponse> UpdateDeletionDateAsync(UserAdditionalInfoRequest request);
        public Task<UserAdditionalInfoResponse> UpdateImageAsync(UserAdditionalInfoRequest request);
    }
}