using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces;
public interface IAuthenticationConfirmService
{
    public Task<TokensResponse> ConfirmAccountAsync(string token, CancellationToken cancellationToken = default);
    public Task<UserResponse> UpdateEmailAsync(string email, string token, CancellationToken cancellationToken = default);
    public Task<UserResponse> UpdateEmailByAdminAsync(Guid userId, string email, CancellationToken cancellationToken = default);
    public Task<UserResponse> UpdateLoginAsync(string login, string token, CancellationToken cancellationToken = default);
    public Task<UserResponse> UpdateLoginByAdminAsync(Guid userId, string login, CancellationToken cancellationToken = default);
    public Task<UserResponse> UpdatePasswordAsync(string password, string token, CancellationToken cancellationToken = default);
    public Task<UserResponse> DeleteAsync(string token, CancellationToken cancellationToken = default);
}