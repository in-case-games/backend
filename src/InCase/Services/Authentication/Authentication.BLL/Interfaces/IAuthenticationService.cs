using Authentication.BLL.Models;
using Authentication.DAL.Entities;

namespace Authentication.BLL.Interfaces;
public interface IAuthenticationService
{
    public Task SignInAsync(UserRequest request, CancellationToken cancellationToken = default);
    public Task SignUpAsync(UserRequest request, CancellationToken cancellationToken = default);
    public Task<TokensResponse> RefreshTokensAsync(string token, CancellationToken cancellationToken = default);
    public Task<User> GetUserFromTokenAsync(string token, string type, CancellationToken cancellationToken = default);
}