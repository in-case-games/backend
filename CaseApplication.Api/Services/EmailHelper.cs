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

        public async Task<bool> SendNotifyToEmail(string email, string subject, string body)
        {
            string? answer = await _emailService.SendToEmail(email, subject, body);

            return answer == null;
        }

        public async Task<bool> SendDeleteAccountToEmail(string email, string userId, string token)
        {
            string subject = "Подтвердите удаление аккаунта";
            string body = $"<b>Link: {_requestUrl}/User/DeleteConfirmation?" +
                $"userId={userId}&token={token}</b>";

            string? answer = await _emailService.SendToEmail(email, subject, body);

            return answer == null;
        }

        public async Task<bool> SendConfirmAccountToEmail(string email, string token)
        {
            string subject = "Подтвердите вход в аккаунт";
            string body = "<b>This is some html text</b>";
            string? answer = await _emailService.SendToEmail(email, subject, body);

            return answer == null;
        }

        public async Task<bool> SendActivationAccountToEmail(string email, string token)
        {
            string subject = "Подтвердите свой аккаунт";
            string body = "<b>This is some html text</b>";
            string? answer = await _emailService.SendToEmail(email, subject, body);

            return answer == null;
        }
    }
}
