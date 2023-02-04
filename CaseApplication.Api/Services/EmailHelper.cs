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
            string body = $"<b>Link: {_requestUrl}/User/DeleteConfirmation?" +
                $"userId={userId}&token={token}</b>";

            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendConfirmAccountToEmail(string email, string token)
        {
            string subject = "Подтвердите вход в аккаунт";
            string body = "<b>This is some html text</b>";
            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendActivationAccountToEmail(string email, string token)
        {
            string subject = "Подтвердите свой аккаунт";
            string body = "<b>This is some html text</b>";
            await _emailService.SendToEmail(email, subject, body);
        }
    }
}
