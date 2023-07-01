using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id);
        public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId);
        public Task<UserAdditionalInfoResponse> UpdateRole(UserAdditionalInfoRequest request);
        public Task<UserAdditionalInfoResponse> UpdateDeletionDate(UserAdditionalInfoRequest request);
        public Task<UserAdditionalInfoResponse> UpdateImage(UserAdditionalInfoRequest request);
    }
}