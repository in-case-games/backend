using CaseApplication.Domain.Entities.Email;
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
            string patternBody = $"<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>" +
                $"<html>" +
                $"<head>" +
                $"<meta http-equiv='Content-type' content='text/html; charset=utf-8'/>" +
                $"</head>" +
                $"<body style='margin: 0; padding: 0;'>" +
                $"<table border='0' cellpadding='0' background='#1A1A1D' bgcolor='#1A1A1D' cellspacing='0' style='margin:0 auto; padding:20px;'>" +
                $"<tr>" +
                $"<td style='width: 150px;'>" +
                $"<span style='position: absolute; top: 50;'>" +
                $"<h3 style='margin: 0; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;'>{title}</h3>" +
                $"<h3 style='margin: 0 0 0 50px; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;'>{subTitle}</h3>" +
                $"</span>" +
                $"</td>" +
                $"<td style='width: 150px;'></td>" +
                $"<td style='width: 150px;'>" +
                $"<img style='width: 100px; float: right;' src='https://sun9-57.userapi.com/impg/Nr9EynFk0eAZC24xIde66jTAgN1UWmz2WefGpQ/NV1F0HXARBc.jpg?size=640x640&quality=96&sign=d55585007324a53f11e43eb27a4a88c4&type=album' alt='logo'></td></tr><tr style='height: 40px;'></tr>" +
                $"<!--BODY-->{body}<!--FOOTER--><tr><td colspan='3' align='center'><hr style='border: 1px solid #BDEED6; margin: 10px 25px;'/></td></tr><tr><td colspan='3' align='center'><span style='display: flex; flex-direction: row; justify-content: space-between; margin: 0px 130px; height: 35px;'>" +
                $"<img style='width: auto' src='https://sun9-48.userapi.com/impg/DvpK2ZpgD59bTa9fkmo2MVOwVkXIFUBQGj_RAA/b-Vw3mzYrTs.jpg?size=50x50&quality=96&sign=178b105556e5dcbcdef7b0bd41d3e14d&type=album' alt='icon'/><img style='width: auto' src='https://sun9-76.userapi.com/impg/p-i7nPtT1hIuc_gGGUXkt_nMxTAV9yMjcjKjMA/mGFmmYQwsuw.jpg?size=50x50&quality=96&sign=49888242985e83e5c4547d5436e5a8f5&type=album' alt='icon'/>" +
                $"<img style='width: auto' src='https://sun9-28.userapi.com/impg/jisRgeRGs8Lyy_GcysYnKJaCbYkhY4tNJNHP3Q/QnvhzMo4bPE.jpg?size=50x50&quality=96&sign=5536357afd05e29201543ad6707271ca&type=album' alt='icon'/>" +
                $"<img style='width: auto' src='https://sun9-44.userapi.com/impg/BSGrpajET4q_xUxqYTzxkib0o90es43hzxckfw/_701WTs4cw4.jpg?size=50x50&quality=96&sign=ab5c79c1d655b054e3d8a2d7a73b5433&type=album' alt='icon'/>" +
                $"</span>" +
                $"<h3 style='margin: 0; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 12px; margin-top: 30px; text-align: center;'>#ПРОМОКОД-НА-ПОПОЛНЕНИЕ-15%-ПРИ-РЕГИСТРАЦИИ</h3></td></tr></table></body></html>";
            return patternBody;
        }
        public async Task SendNotifyToEmail(string email, string subject, EmailTemplate emailPattern)
        {
            string body = CreatePatternBody(subject, emailPattern.Body);
            await _emailService.SendToEmail(email, subject, body);
        }
        public async Task SendSignUpAccountToEmail(DataMailLink emailModel)
        {
            string subject = "Вы успешно зарегистрированы!";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<tr> <td colspan='3' style='width: 180px;'><h4 style='color: #BDEED6; text-align: center; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 21px;'>Дорогой <i style='color: #AB333B; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 18px;'>{{userName}}</i></h4></td></tr><tr> <td colspan='3' ><p style='text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; position: relative;'>" +
                $"Для завершения этапа регистрации, вам необходимо нажать на кнопку ниже для подтверждения почты. Если это были не вы, проигнорируйте это сообщение." +
                $"<br/>С уважением команда InCase </p></td></tr><tr><td colspan='3' align='center'><button type='Submit' style='margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'MS Sans Serif'; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;'><a href='{uri}' style='text-decoration: none; color: #BDEED6'>Подверждаю</a></button></td></tr>";
            string body = CreatePatternBody("Завершение", subTitle: "регистрации", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendSignInAccountToEmail(DataMailLink emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<span style='display: flex; flex-direction: column; justify-content: space-between; align-items: center;'><h4 style='color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;'>Дорогой {userName}</h4><p style='text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;'>В ваш аккаунт был произведен вход.Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств<br/>С уважением команда InCase </p><a href='{uri}'><button type='Submit' style='cursor: pointer; background-color: transparent; font-family: 'MS Sans Serif'; font-weight: bold; padding: 10px 75px; margin: 20px 0px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;'>Подверждаю</button></a></span>";
            string body = CreatePatternBody("Потдверждение", subTitle: "входа", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendConfirmationAccountToEmail(DataMailLink emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string patternBody = $"<span style='display: flex; flex-direction: column; justify-content: space-between; align-items: center;'><h4 style='color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;'>Добро пожаловать, {userName}</h4><p style='text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;'>Мы рады, что вы новый участник нашего проекта.Надеемся, что вам понравится наша реализация открытия кейсов и подарит множество эмоций и новых предметовС уважением команда InCase<br/>С уважением команда InCase </p></span>"; 
            string body = CreatePatternBody("Потдверждение", subTitle: "регистрации", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendAccountLoginAttempt(DataMailLink emailModel, string userName)
        {
            string subject = "Попытка входа.";
            string patternBody = $"<span style='display: flex; flex-direction: column; justify-content: space-between; align-items: center;'><h4 style='color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold; font-size: 20px;'>Добро пожаловать, {userName}</h4><p style='text-align: center; color: #BDEED6; font-family: 'MS Sans Serif'; font-weight: bold;'>Мы рады, что вы новый участник нашего проекта.В ваш аккаунт был произведен вход.Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств<br/>С уважением команда InCase </p></span>";
            string body = CreatePatternBody("Попытка", subTitle: "входа", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendDeleteAccountToEmail(DataMailLink emailModel)
        {
            string subject = "Подтвердите удаление аккаунта";
            string body =

                $"<b>" +
                $"Link: {_requestUrl}/User/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangePasswordToEmail(DataMailLink emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body =
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
        public async Task SendChangeEmailToEmail(DataMailLink emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string body =
                $"<b>" +
                $"Link: {_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}" +
                $"</b>";

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendConfirmAccountToEmail(DataMailLink emailModel)
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
