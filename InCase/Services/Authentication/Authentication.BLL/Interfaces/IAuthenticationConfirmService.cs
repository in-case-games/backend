using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationConfirmService
    {
        public Task<TokensResponse> ConfirmAccount(string token);
        public Task<UserResponse> UpdateEmail(string email, string token);
        public Task<UserResponse> UpdatePassword(string password, string token);
        public Task<UserResponse> Delete(string token);
    }
}
