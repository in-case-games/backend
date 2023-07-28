using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(Guid id);
        public Task<UserResponse> GetAsync(string login);
        public Task<User?> GetByConsumerAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task<UserResponse> UpdateLoginAsync(UserRequest request);
        public Task DeleteAsync(Guid id);
    }
}
