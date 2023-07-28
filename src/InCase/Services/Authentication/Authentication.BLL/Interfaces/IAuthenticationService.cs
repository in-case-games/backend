using Authentication.BLL.Models;
using Authentication.DAL.Entities;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        public Task SignInAsync(UserRequest request);
        public Task SignUpAsync(UserRequest request);
        public Task<TokensResponse> RefreshTokensAsync(string token);
        public Task<User> GetUserFromTokenAsync(string token, string type);
        public Task CheckUserForBanAsync(Guid id);
    }
}
