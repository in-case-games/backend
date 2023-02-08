using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.Api.Services
{
    //TODO combine all notification methods into one, make the transfer of the model (header main footer info)
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
        public async Task SendNotifyChangeLogin(string email, string login)
        {
            await SendNotifyToEmail(
                email, 
                "Администрация сайта", 
                $"<b>Имя вашего акканута измененно на: {login}</b>");
        }

        public async Task SendNotifyDeleteAccount(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Ваш аккаунт будет удален через 30 дней</b>");
        }

        public async Task SendNotifyChangeEmail(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Вы изменили email аккаунта</b>");
        }
        public async Task SendNotifyActivateEmail(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Спасибо что подтвердили аккаунт</b>");
        }
        public async Task SendNotifyAttemptSingIn(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Попытка входа в аккаунт</b>");
        }

        public async Task SendNotifyChangePassword(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Вы сменили пароль</b>");
        }

        public async Task SendNotifyAccountSignIn(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Вход в аккаунт</b>");
        }

        public async Task SendNotifyAccountSignUp(string email)
        {
            await SendNotifyToEmail(email, "Администрация сайта", "<b>Поздравляем о регистрации</b>");
        }

        public async Task SendDeleteAccountToEmail(EmailModel emailModel)
        {
            string subject = "Подтвердите удаление аккаунта";
            string body = 
                $"<b>" +
                $"Link: {_requestUrl}/User/" +
                $"{emailModel.UserId}&{emailModel.UserToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangePasswordToEmail(EmailModel emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body = 
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.UserToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendChangeEmailToEmail(EmailModel emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body = 
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.UserToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendConfirmAccountToEmail(EmailModel emailModel)
        {
            string subject = "Подтвердите вход в аккаунт";
            string body = $"<b>" +
                $"Link: {_requestUrl}/User/" +
                $"{emailModel.UserId}&{emailModel.UserToken}&{emailModel.UserIp}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendActivateAccountToEmail(EmailModel emailModel)
        {
            string subject = "Подтвердите свой email аккаунт";
            string body = $"<b>" +
                $"Link: {_requestUrl}/User/" +
                $"{emailModel.UserId}&{emailModel.UserToken}&{emailModel.UserIp}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
    }
}
