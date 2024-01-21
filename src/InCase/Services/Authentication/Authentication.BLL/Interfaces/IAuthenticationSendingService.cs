using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces;

public interface IAuthenticationSendingService
{
    public Task ForgotPasswordAsync(DataMailRequest request, CancellationToken cancellationToken = default);
    public Task UpdateLoginAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default);
    public Task UpdateEmailAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default);
    public Task UpdatePasswordAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default);
    public Task DeleteAccountAsync(DataMailRequest request, string password, CancellationToken cancellationToken = default);
}