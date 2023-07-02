using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> Get(Guid id);
        public Task<UserResponse> Get(string login);
        public Task<UserResponse> UpdateLogin(UserRequest request);
    }
}
