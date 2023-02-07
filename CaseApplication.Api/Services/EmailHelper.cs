﻿using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.Api.Services
{
    public class EmailHelper
    {
        private readonly EmailService _emailService;
        private readonly string _requestUrl = "https://localhost:7053";

        public EmailHelper(EmailService emailService)
        {

            _emailService = emailService;
        }

        public async Task SendNotifyToEmail(string email, string subject, string body)
        {
            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendDeleteAccountToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите удаление аккаунта";
            string body = $"<b>Link: {_requestUrl}/User/userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendChangePasswordToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите изменение пароля";
            string body = $"<b>Link: {_requestUrl}/User/userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }
        public async Task SendChangeEmailToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите изменение пароля";
            string body = $"<b>Link: {_requestUrl}/User/userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendConfirmAccountToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите вход в аккаунт";
            string body = $"<b>Link: {_requestUrl}/User/userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendActivateAccountToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите свой email аккаунт";
            string body = $"<b>Link: {_requestUrl}/User/userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }
    }
}
