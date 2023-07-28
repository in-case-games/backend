using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserRoleService
    {
        public Task<UserRoleResponse> GetAsync(Guid id);
        public Task<List<UserRoleResponse>> GetAsync();
    }
}
