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

        private static string CreateEmailTemplate(string title, string subtitle, string body)
        {
            string patternBody = $"<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>\r\n<html>\r\n<head>\r\n<meta http-equiv='Content-type' content='text/html; charset=utf-8'/>\r\n</head>\r\n<body>\r\n<div style=\"margin:0;padding:0\" bgcolor=\"#FFFFFF\">\r\n<table width=\"100%\" height=\"100%\" style=\"min-width:348px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" lang=\"en\">\r\n<tbody>\r\n<tr height=\"32\" style=\"height:32px\">\r\n<td></td>\r\n</tr>\r\n<tr align=\"center\">\r\n<td>\r\n<div>\r\n<div></div>\r\n</div>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"padding-bottom:20px;max-width:516px;min-width:220px;color: #BDEED6;\" bgcolor=\"#1A1A1D\">\r\n<tbody>\r\n<tr>\r\n<td width=\"8\" style=\"width:8px\"></td>\r\n<td>\r\n<div style=\"padding:40px 20px\" align=\"center\">\r\n<!--Header-->\r\n<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:40px;text-align:center;word-break:break-word\">\r\n<table align=\"center\">\r\n<tbody>\r\n<tr style=\"line-height:normal;\">\r\n<td align=\"left\">\r\n<h3 style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:20px;line-height:20px;margin: 0;color: #BDEED6;\">{title}</h3>\r\n<h3 style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:20px;line-height:20px;margin: 0 0 0 50px;color: #BDEED6;\">{subtitle}</h3>\r\n</td>\r\n<td width=\"150px\"></td>\r\n<td align=\"right\">\r\n<img src=\"https://sun9-57.userapi.com/impg/Nr9EynFk0eAZC24xIde66jTAgN1UWmz2WefGpQ/NV1F0HXARBc.jpg?size=640x640&quality=96&sign=d55585007324a53f11e43eb27a4a88c4&type=album\" width=\"100\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\">\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n<!--Body-->\r\n{body}\r\n<!--FOOTER-->\r\n<div style=\"text-align:left; padding-top: 60px;\">\r\n<hr style=\"border: 1px solid #BDEED6; margin: 10px 50px;\">\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:14px;color: #BDEED6;line-height:18px;text-align:center\">\r\n<table align=\"center\" style=\"padding-bottom:20px;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<a href=\"https://yandex.ru\">\r\n<img style=\"cursor: pointer;\" src=\"https://sun9-48.userapi.com/impg/DvpK2ZpgD59bTa9fkmo2MVOwVkXIFUBQGj_RAA/b-Vw3mzYrTs.jpg?size=50x50&quality=96&sign=178b105556e5dcbcdef7b0bd41d3e14d&type=album\" width=\"45\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/>\r\n</a>\r\n</td>\r\n<td>\r\n<a href=\"https://yandex.ru\">\r\n<img style=\"cursor: pointer;\" src=\"https://sun9-76.userapi.com/impg/p-i7nPtT1hIuc_gGGUXkt_nMxTAV9yMjcjKjMA/mGFmmYQwsuw.jpg?size=50x50&quality=96&sign=49888242985e83e5c4547d5436e5a8f5&type=album\" href=\"yandex.ru\" width=\"34\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/>\r\n</a>\r\n</td>\r\n<td>\r\n<a href=\"https://yandex.ru\">\r\n<img style=\"cursor: pointer;\" src=\"https://sun9-28.userapi.com/impg/jisRgeRGs8Lyy_GcysYnKJaCbYkhY4tNJNHP3Q/QnvhzMo4bPE.jpg?size=50x50&quality=96&sign=5536357afd05e29201543ad6707271ca&type=album\" href=\"yandex.ru\" width=\"35\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/>\r\n</a>\r\n</td>\r\n<td>\r\n<a href=\"https://yandex.ru\">\r\n<img style=\"cursor: pointer;\" src=\"https://sun9-44.userapi.com/impg/BSGrpajET4q_xUxqYTzxkib0o90es43hzxckfw/_701WTs4cw4.jpg?size=50x50&quality=96&sign=ab5c79c1d655b054e3d8a2d7a73b5433&type=album\" href=\"yandex.ru\" width=\"41\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/>\r\n</a>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n#ПРОМОКОД-НА-ПОПОЛНЕНИЕ-15%-ПРИ-РЕГИСТРАЦИИ\r\n</div>\r\n</div>\r\n</div>\r\n</td>\r\n<td width=\"8\" style=\"width:8px\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr height=\"32\" style=\"height:32px\">\r\n<td></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</body>\r\n</html>";
            return patternBody;
        }

        public async Task SendNotifyToEmail(string email, string subject, EmailTemplate emailTemplate)
        {
            //TODO
            string body = CreateEmailTemplate(
                emailTemplate.HeaderTittle, 
                emailTemplate.HeaderSubTittle,
                emailTemplate.BodyDescription);
            await _emailService.SendToEmail(email, subject, body);
        }

        public async Task SendSignUp(DataMailLink emailModel, string userName)
        {
            string subject = "Подтверждение регистрации.";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Дорогой {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nДля завершения этапа регистрации, вам необходимо нажать на кнопку ниже для подтверждения почты. \r\nЕсли это были не вы, проигнорируйте это сообщение.\r\n</div><div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n<div style=\"padding-top:50px;text-align:center\">\r\n<a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\nПодтвердить\r\n</a>\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Завершение", "регистрации", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendSignIn(DataMailLink emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string uri = $"{_requestUrl}/email/api/EmailTokenReceive/confirm/" +
                $"{emailModel.UserId}&{emailModel.EmailToken}" +
                $"?ip={emailModel.UserIp}&platform={emailModel.UserPlatforms}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n    <div style=\"font-size:22px; color: #BDEED6;\">Дорогой {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n    <div style=\"color: #BDEED6;\">\r\n        Подтвердите вход в аккаунт с устройства {emailModel.UserPlatforms}. \r\n        Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.\r\n    </div>\r\n    <div style=\"color: #BDEED6;\">\r\n        С уважением команда InCase\r\n    </div>\r\n    <div style=\"padding-top:50px;text-align:center\">\r\n        <a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\n            Подтвердить\r\n        </a>\r\n    </div>\r\n</div>";
            string body = CreateEmailTemplate("Подтверждение", "входа", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendSuccessVerifedAccount(DataMailLink emailModel, string userName)
        {
            string subject = "Подтверждение входа.";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Добро пожаловать, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nМы рады, что вы новый участник нашего проекта.\r\nНадеемся, что вам понравится наша реализация открытия кейсов и подарит множество эмоций и новых предметов\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n</div>"; 
            string body = CreateEmailTemplate("Подтверждение", "регистрации", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendLoginAttempt(DataMailLink emailModel, string userName)
        {
            string subject = "Вход в аккаунт.";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Добро пожаловать, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nВ ваш аккаунт вошли с {emailModel.UserPlatforms}. \r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Вход", "в аккаунт", body: patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendDeleteAccount(DataMailLink emailModel, string userName)
        {
            string subject = "Подтвердите удаление аккаунта";
            string uri = $"{_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Внимание, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nПодтвердите, что это вы удаляете аккаунт. \r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.\r\nМы удалим ваш аккаунт при достижении 30 дней с момента нажатия на эту кнопку.\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n<div style=\"padding-top:50px;text-align:center\">\r\n<a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\nПодтвердить\r\n</a>\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Удаление", "аккаунта", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangePassword(DataMailLink emailModel, string userName)
        {
            string subject = "Подтвердите изменение пароля";
            string uri = $"{_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Внимание, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nПодтвердите, что это вы хотите поменять пароль с устройства {emailModel.UserPlatforms}. \r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n<div style=\"padding-top:50px;text-align:center\">\r\n<a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\nПодтвердить\r\n</a>\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Смена", "пароля", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangeEmail(DataMailLink emailModel, string userName)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Внимание, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nПодтвердите, что это вы хотите поменять email с устройства {emailModel.UserPlatforms}. \r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n<div style=\"padding-top:50px;text-align:center\">\r\n<a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\nПодтвердить\r\n</a>\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Смена", "почты", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendConfirmNewEmail(DataMailLink emailModel, string userName)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/User/{emailModel.UserId}&{emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\">\r\n<div style=\"font-size:22px; color: #BDEED6;\">Дорогой, {userName}</div>\r\n</div>\r\n<div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\">\r\n<div style=\"color: #BDEED6;\">\r\nПодтвердите, что это ваш новый email. Отправка с устройства {emailModel.UserPlatforms}. \r\nЕсли это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств\r\n</div>\r\n<div style=\"color: #BDEED6;\">\r\nС уважением команда InCase\r\n</div>\r\n<div style=\"padding-top:50px;text-align:center\">\r\n<a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">\r\nПодтвердить\r\n</a>\r\n</div>\r\n</div>";
            string body = CreateEmailTemplate("Смена", "почты", patternBody);

            await _emailService.SendToEmail(emailModel.UserEmail, subject, body);
        }
    }
}
