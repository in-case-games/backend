using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationConfirmService
    {
        public Task<TokensResponse> ConfirmAccountAsync(string token);
        public Task<UserResponse> UpdateEmailAsync(string email, string token);
        public Task<UserResponse> UpdateLoginAsync(string login, string token);
        public Task<UserResponse> UpdatePasswordAsync(string password, string token);
        public Task<UserResponse> DeleteAsync(string token);
    }
}
