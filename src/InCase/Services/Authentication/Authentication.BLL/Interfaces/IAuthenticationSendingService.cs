using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationSendingService
    {
        public Task ForgotPasswordAsync(DataMailRequest request);
        public Task UpdateLoginAsync(DataMailRequest request, string password);
        public Task UpdateEmailAsync(DataMailRequest request, string password);
        public Task UpdatePasswordAsync(DataMailRequest request, string password);
        public Task DeleteAccountAsync(DataMailRequest request, string password);
    }
}
