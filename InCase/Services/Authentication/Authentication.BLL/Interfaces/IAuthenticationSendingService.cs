﻿using Authentication.BLL.Models;

namespace Authentication.BLL.Interfaces
{
    public interface IAuthenticationSendingService
    {
        public Task ConfirmAccountAsync(DataMailRequest request, string password);
        public Task ConfirmNewEmailAsync(DataMailRequest request, string email);
        public Task ForgotPasswordAsync(DataMailRequest request);
        public Task UpdateEmailAsync(DataMailRequest request, string password);
        public Task UpdatePasswordAsync(DataMailRequest request, string password);
        public Task DeleteAccountAsync(DataMailRequest request, string password);
    }
}
