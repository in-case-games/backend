using Identity.BLL.Models;

namespace Identity.BLL.Interfaces;
public interface IUserRoleService
{
    public Task<UserRoleResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<UserRoleResponse>> GetAsync(CancellationToken cancellation = default);
}