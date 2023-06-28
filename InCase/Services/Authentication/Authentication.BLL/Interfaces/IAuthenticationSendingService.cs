using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationSendingService
    {
        public Task ConfirmAccount(DataMailRequest request, string password);
        public Task ConfirmNewEmail(DataMailRequest request, string email);
        public Task ForgotPassword(DataMailRequest request);
        public Task UpdateEmail(DataMailRequest request, string password);
        public Task UpdatePassword(DataMailRequest request, string password);
        public Task DeleteAccount(DataMailRequest request, string password);
    }
}
