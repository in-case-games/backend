using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationConfirmService
    {
        public Task<TokensResponse> ConfirmAccountAsync(string token);
        public Task<UserResponse> UpdateEmailAsync(string email, string token);
        public Task<UserResponse> UpdateEmailByAdminAsync(Guid userId, string email);
        public Task<UserResponse> UpdateLoginAsync(string login, string token);
        public Task<UserResponse> UpdateLoginByAdminAsync(Guid userId, string login);
        public Task<UserResponse> UpdatePasswordAsync(string password, string token);
        public Task<UserResponse> DeleteAsync(string token);
    }
}
