using Authentication.BLL.Models;
using Authentication.DAL.Entities;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        public Task SignIn(UserRequest request);
        public Task SignUp(UserRequest request);
        public Task<TokensResponse> RefreshTokens(string token);
        public Task<User> GetUserFromToken(string token, string type);
        public Task CheckUserForBan(Guid id);
    }
}
