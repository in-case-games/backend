using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<List<UserResponse>> GetAsync(int range, CancellationToken cancellationToken = default);
        public Task<List<UserRoleResponse>> GetRolesAsync(CancellationToken cancellationToken = default);
        public Task UpdateAsync(UserAdditionalInfoRequest info, CancellationToken cancellationToken = default);
    }
}