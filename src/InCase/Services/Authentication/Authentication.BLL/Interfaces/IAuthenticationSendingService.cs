using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationSendingService
    {
        public Task ForgotPasswordAsync(string login);
        public Task UpdateLoginAsync(string login, string password);
        public Task UpdateEmailAsync(string login, string password);
        public Task UpdatePasswordAsync(string login, string password);
        public Task DeleteAccountAsync(string login, string password);
    }
}
