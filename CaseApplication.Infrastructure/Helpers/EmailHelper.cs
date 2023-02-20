using CaseApplication.Domain.Entities.External;
using CaseApplication.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace CaseApplication.Infrastructure.Helpers
{
    //TODO combine all notification methods into one, make the transfer of the model (header main footer info)
    public class EmailHelper
    {
        private readonly EmailService _emailService;
        private readonly string _requestUrl = "https://localhost:7053";
        public EmailHelper(
            EmailService emailService, 
            IConfiguration configuration)
        {

            _emailService = emailService;
            _requestUrl = configuration["EmailConfig:AddressCallback"]!;
        }
        private static string CreatePatternBody(EmailMessagePattern emailPattern)
        {
            string patternBody = $"<center class='letter-wrapper' style='width: 100%; table-layout: fixed; text-align: center; -webkit-text-size-adjust: 100%; text-align: center;'><div class='webkit' style='margin: 0 auto; width: 100%; max-width: 600px; -webkit-text-size-adjust: 100%; display: block;'><table class='presentation' cellspacing='0' cellpadding='0' border='0' align='center' style='background-color: #1A1A1D; max-width: 600px; width: 100%; margin: 0 auto; -ms-text-size-adjust: 100%; border-spacing: 0; font-family: Arial, sans-serif; font-style: normal; font-weight: 400; color: #5a5a5a; '><tbody><tr style='text-align: center; padding: 30px 30px;'><td>{emailPattern.Header}</td></tr><tr style='text-align: center; padding: 0px 30px;'><td>{emailPattern.Body}</td></tr><tr style='text-align: center; padding: 30px 30px;'><td>{emailPattern.Footer}</td></tr></tbody></table></div></center>";
            return patternBody;
        }
        public async Task SendNotifyToEmail(string email, string subject, EmailMessagePattern emailPattern)
        {
            string body = CreatePatternBody(emailPattern);
            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendDeleteAccountToEmail(EmailPattern emailModel)
        {
            string subject = "Подтвердите удаление аккаунта";
            string body =
                $"<b>" +
                $"Link: {_requestUrl}/User/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangePasswordToEmail(EmailPattern emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body =
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendChangeEmailToEmail(EmailPattern emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body =
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendConfirmAccountToEmail(EmailPattern emailModel)
        {
            string subject = "Подтвердите вход в аккаунт";
            string body = $"<b>" +
                $"Link: {_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
    }
}
