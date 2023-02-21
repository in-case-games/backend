using CaseApplication.Domain.Entities.External;
using CaseApplication.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace CaseApplication.Infrastructure.Helpers
{
    //TODO combine all notification methods into one, make the transfer of the model (header main footer info)
    public class EmailHelper
    {
        private readonly EmailService _emailService;
        private readonly string _requestUrl = "https://localhost:7138";
        public EmailHelper(
            EmailService emailService,
            IConfiguration configuration)
        {
            _requestUrl = configuration["EmailConfig:AddressCallback"]!;
            _emailService = emailService;
        }
        private static string CreatePatternBody(string title, string body, string subTitle = "")
        {
            string patternBody = $"<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd'>\r\n<html>\r\n<head>\r\n<meta http-equiv='Content-type' content='text/html; charset=utf-8'/>\r\n</head>\r\n<body style='margin: 0; padding: 0; max-width: 500px; min-width: 500px;'><div style=\"display:none; white-space:nowrap; font:15px courier; line-height:0;\"> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </div>\r\n<table border=\"0\" cellpadding=\"0\" background=\"#1A1A1D\" bgcolor=\"#1A1A1D\" cellspacing=\"0\" style=\"margin:0 auto; padding:0\" style='border: collapse; border: 1px; max-width: 500px; min-width: 500px; align-items: center; display: flex; flex-direction: column; border-radius: 15px;'>\r\n<tr>\r\n<td>\r\n<center style=\"max-width: 500px; min-width: 500px; width: 100%;\">\r\n<table background=\"#1A1A1D\" bgcolor=\"#1A1A1D\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin:0; margin-bottom: 60px; padding:30px\" width=\"100%\">\r\n<tr>\r\n<td>\r\n<!--HEADER-->\r\n<span style=\"display: flex; flex-direction: row; justify-content: space-between; align-items: center; margin-bottom: 40px;\">\r\n<div>\r\n<h3 style=\"margin: 0; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">{title}</h3>\r\n<h3 style=\"margin: 0; margin-left: 50px; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">{subTitle}</h3>\r\n</div>\r\n<img style=\"width: 20%\" src=\"https://sun9-57.userapi.com/impg/Nr9EynFk0eAZC24xIde66jTAgN1UWmz2WefGpQ/NV1F0HXARBc.jpg?size=640x640&quality=96&sign=d55585007324a53f11e43eb27a4a88c4&type=album\" alt=\"logo\">\r\n</span>\r\n<!--BODY-->{body}\r\n\n<hr style=\"border: 1px solid #BDEED6; margin: 30px 25px;\"/>\n<!--FOOTER-->\r\n<span style=\"display: flex; flex-direction: row; justify-content: space-between; margin: 0px 130px; height: 35px;\">\r\n<img style=\"width: auto\" src=\"https://sun9-48.userapi.com/impg/DvpK2ZpgD59bTa9fkmo2MVOwVkXIFUBQGj_RAA/b-Vw3mzYrTs.jpg?size=50x50&quality=96&sign=178b105556e5dcbcdef7b0bd41d3e14d&type=album\" alt=\"icon\"/>\r\n<img style=\"width: auto\" src=\"https://sun9-76.userapi.com/impg/p-i7nPtT1hIuc_gGGUXkt_nMxTAV9yMjcjKjMA/mGFmmYQwsuw.jpg?size=50x50&quality=96&sign=49888242985e83e5c4547d5436e5a8f5&type=album\" alt=\"icon\"/>\r\n<img style=\"width: auto\" src=\"https://sun9-28.userapi.com/impg/jisRgeRGs8Lyy_GcysYnKJaCbYkhY4tNJNHP3Q/QnvhzMo4bPE.jpg?size=50x50&quality=96&sign=5536357afd05e29201543ad6707271ca&type=album\" alt=\"icon\"/>\r\n<img style=\"width: auto\" src=\"https://sun9-44.userapi.com/impg/BSGrpajET4q_xUxqYTzxkib0o90es43hzxckfw/_701WTs4cw4.jpg?size=50x50&quality=96&sign=ab5c79c1d655b054e3d8a2d7a73b5433&type=album\" alt=\"icon\"/>\r\n</span>\r\n<h3 style=\"margin: 0; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 12px; margin-top: 30px; text-align: center;\">#ПРОМОКОД-НА-ПОПОЛНЕНИЕ-15%-ПРИ-РЕГИСТРАЦИИ</h3>\r\n</td>\r\n</tr>\r\n</table>\r\n</center>\r\n</td>\r\n</tr>\r\n</table>\r\n</body>\r\n</html>";
            return patternBody;
        }
        public async Task SendNotifyToEmail(string email, string subject, EmailMessagePattern emailPattern)
        {
            string body = CreatePatternBody(subject, emailPattern.Body);
            await _emailService.SendToEmail(email, subject, body);
        }
        public async Task SendSignUpAccountToEmail(EmailPattern emailModel)
        {
            string subject = "Вы успешно зарегистрированы!";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<span style=\"display: flex; flex-direction: column; justify-content: space-between; align-items: center;\">\r\n<h4 style=\"color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;\">Добро пожаловать в InCase</h4>\r\n<p style=\"text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">Для завершения этапа регистрации, вам необходимо\r\nнажать на кнопку ниже для подтверждения почты. <br/>\r\n<b style=\"color: #AB333B\">Если это были не вы, проигнорируйте это сообщение. </b><br/>\r\nС уважением команда InCase </p>\r\n<a href=\"{uri}\"><button style=\"cursor: pointer; background-color: transparent; font-family: 'MS Sans Serif'; font-weight: bold; padding: 10px 75px; margin: 20px 0px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\">Подверждаю</button></a>\r\n</span>";
            string body = CreatePatternBody("Завершение", subTitle: "регистрации", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendSignInAccountToEmail(EmailPattern emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<span style=\"display: flex; flex-direction: column; justify-content: space-between; align-items: center;\">\r\n<h4 style=\"color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;\">Дорогой {userName}</h4>\r\n<p style=\"text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">В ваш аккаунт был произведен вход.\r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств\r\n<br/>\r\nС уважением команда InCase </p>\r\n<a href=\"{uri}\"><button type=\"Submit\" style=\"cursor: pointer; background-color: transparent; font-family: 'MS Sans Serif'; font-weight: bold; padding: 10px 75px; margin: 20px 0px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\">Подверждаю</button></a>\r\n</span>";
            string body = CreatePatternBody("Потдверждение", subTitle: "входа", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendConfirmationAccountToEmail(EmailPattern emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string patternBody = $"<span style=\"display: flex; flex-direction: column; justify-content: space-between; align-items: center;\">\r\n<h4 style=\"color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;\">Добро пожаловать, {userName}</h4><p style=\"text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">Мы рады, что вы новый участник нашего проекта.\r\nНадеемся, что вам понравится наша реализация открытия кейсов и подарит множество эмоций и новых предметов\r\nС уважением команда InCase<br/>С уважением команда InCase </p>\r\n</span>"; 
            string body = CreatePatternBody("Потдверждение", subTitle: "регистрации", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendAccountLoginAttempt(EmailPattern emailModel, string userName)
        {
            string subject = "Попытка входа.";
            string patternBody = $"<span style=\"display: flex; flex-direction: column; justify-content: space-between; align-items: center;\">\r\n<h4 style=\"color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;\">Добро пожаловать, {userName}</h4><p style=\"text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;\">Мы рады, что вы новый участник нашего проекта.\r\nВ ваш аккаунт был произведен вход.\r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств<br/>С уважением команда InCase </p>\r\n</span>";
            string body = CreatePatternBody("Попытка", subTitle: "входа", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
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
