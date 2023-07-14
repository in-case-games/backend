using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(Guid id);
        public Task<UserResponse> CreateAsync(UserRequest request, bool IsNewGuid = false);
        public Task<UserResponse> UpdateAsync(UserRequest request);
        public Task<UserResponse> DeleteAsync(Guid id);
    }
}
