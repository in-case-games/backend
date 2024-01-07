using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id, CancellationToken cancellation = default);
        public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
        public Task<UserAdditionalInfoResponse> UpdateRoleAsync(Guid userId, Guid roleId, CancellationToken cancellation = default);
        public Task<UserAdditionalInfoResponse> UpdateDeletionDateAsync(Guid userId, DateTime? deletionDate, CancellationToken cancellation = default);
        public Task<UserAdditionalInfoResponse> UpdateImageAsync(UpdateImageRequest request, CancellationToken cancellation = default);
    }
}