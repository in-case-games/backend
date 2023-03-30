using InCase.Domain.Entities.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace InCase.Infrastructure.Services
{
    public class EmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _smtpEmail;
        private readonly string _smtpPassword;
        private readonly string _requestUrl;

        public EmailService(IConfiguration configuration)
        {
            _host = configuration["EmailConfig:Host"]!;
            _port = int.Parse(configuration["EmailConfig:Port"]!);
            _smtpEmail = configuration["EmailConfig:Email"]!;
            _smtpPassword = configuration["EmailConfig:Password"]!;
            _requestUrl = configuration["EmailConfig:AddressCallback"]!;
        }

        private static string CreateEmailTemplate(string title, string subtitle, string body)
        {
            return $"<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'><html><head><meta http-equiv='Content-type' content='text/html; charset=utf-8'/></head><body><div style=\"margin:0;padding:0\" bgcolor=\"#FFFFFF\"><table width=\"100%\" height=\"100%\" style=\"min-width:348px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" lang=\"en\"><tbody><tr height=\"32\" style=\"height:32px\"><td></td></tr><tr align=\"center\"><td><div><div></div></div><table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"padding-bottom:20px;max-width:516px;min-width:220px;color: #BDEED6;\" bgcolor=\"#1A1A1D\"><tbody><tr><td width=\"8\" style=\"width:8px\"></td><td><div style=\"padding:40px 20px\" align=\"center\"><!--Header--><div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:40px;text-align:center;word-break:break-word\"><table align=\"center\"><tbody><tr style=\"line-height:normal;\"><td align=\"left\"><h3 style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:20px;line-height:20px;margin: 0;color: #BDEED6;\">{title}</h3><h3 style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:20px;line-height:20px;margin: 0 0 0 50px;color: #BDEED6;\">{subtitle}</h3></td><td width=\"150px\"></td><td align=\"right\"><img src=\"https://sun9-57.userapi.com/impg/Nr9EynFk0eAZC24xIde66jTAgN1UWmz2WefGpQ/NV1F0HXARBc.jpg?size=640x640&quality=96&sign=d55585007324a53f11e43eb27a4a88c4&type=album\" width=\"100\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"></td></tr></tbody></table></div><!--Body-->{body}<!--FOOTER--><div style=\"text-align:left; padding-top: 60px;\"><hr style=\"border: 1px solid #BDEED6; margin: 10px 50px;\"><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:14px;color: #BDEED6;line-height:18px;text-align:center\"><table align=\"center\" style=\"padding-bottom:20px;\"><tbody><tr><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;\" src=\"https://sun9-48.userapi.com/impg/DvpK2ZpgD59bTa9fkmo2MVOwVkXIFUBQGj_RAA/b-Vw3mzYrTs.jpg?size=50x50&quality=96&sign=178b105556e5dcbcdef7b0bd41d3e14d&type=album\" width=\"45\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;\" src=\"https://sun9-76.userapi.com/impg/p-i7nPtT1hIuc_gGGUXkt_nMxTAV9yMjcjKjMA/mGFmmYQwsuw.jpg?size=50x50&quality=96&sign=49888242985e83e5c4547d5436e5a8f5&type=album\" href=\"yandex.ru\" width=\"34\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;\" src=\"https://sun9-28.userapi.com/impg/jisRgeRGs8Lyy_GcysYnKJaCbYkhY4tNJNHP3Q/QnvhzMo4bPE.jpg?size=50x50&quality=96&sign=5536357afd05e29201543ad6707271ca&type=album\" href=\"yandex.ru\" width=\"35\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;\" src=\"https://sun9-44.userapi.com/impg/BSGrpajET4q_xUxqYTzxkib0o90es43hzxckfw/_701WTs4cw4.jpg?size=50x50&quality=96&sign=ab5c79c1d655b054e3d8a2d7a73b5433&type=album\" href=\"yandex.ru\" width=\"41\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td></tr></tbody></table>#ПРОМОКОД-НА-ПОПОЛНЕНИЕ-15%-ПРИ-РЕГИСТРАЦИИ</div></div></div></td><td width=\"8\" style=\"width:8px\"></td></tr></tbody></table></td></tr><tr height=\"32\" style=\"height:32px\"><td></td></tr></tbody></table></div></body></html>";
        }

        public async Task SendNotifyToEmail(string email, string subject, EmailTemplate emailTemplate)
        {
            //TODO
            string body = CreateEmailTemplate(
                emailTemplate.HeaderTittle,
                emailTemplate.HeaderSubTittle,
                emailTemplate.BodyDescription);
            await SendToEmail(email, subject, body);
        }

        public async Task SendSignUp(DataMailLink emailModel)
        {
            string subject = "Подтверждение регистрации.";
            string uri = $"{_requestUrl}/email/receive?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Дорогой {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Для завершения этапа регистрации, вам необходимо нажать на кнопку ниже для подтверждения почты. Если это были не вы, проигнорируйте это сообщение.</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Завершение", "регистрации", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendSignIn(DataMailLink emailModel)
        {
            string subject = "Подтверждение входа.";
            string uri = $"{_requestUrl}/email/receive?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Дорогой {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Подтвердите вход в аккаунт с устройства {emailModel.UserPlatforms}.Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Подтверждение", "входа", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendSuccessVerifedAccount(DataMailLink emailModel)
        {
            string subject = "Подтверждение входа.";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Добро пожаловать, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Мы рады, что вы новый участник нашего проекта.Надеемся, что вам понравится наша реализация открытия кейсов и подарит множество эмоций и новых предметов</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div></div>";
            string body = CreateEmailTemplate("Подтверждение", "регистрации", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendLoginAttempt(DataMailLink emailModel)
        {
            string subject = "Вход в аккаунт.";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Добро пожаловать, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">В ваш аккаунт вошли с {emailModel.UserPlatforms}. Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div></div>";
            string body = CreateEmailTemplate("Вход", "в аккаунт", body: patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendDeleteAccount(DataMailLink emailModel)
        {
            string subject = "Подтвердите удаление аккаунта";
            string uri = $"{_requestUrl}/email/receive/delete?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Внимание, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Подтвердите, что это вы удаляете аккаунт. Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств.Мы удалим ваш аккаунт при достижении 30 дней с момента нажатия на эту кнопку.</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Удаление", "аккаунта", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangePassword(DataMailLink emailModel)
        {
            string subject = "Подтвердите изменение пароля";
            string uri = $"{_requestUrl}/email/receive/update/password?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Внимание, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Подтвердите, что это вы хотите поменять пароль с устройства {emailModel.UserPlatforms}. Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Смена", "пароля", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendChangeEmail(DataMailLink emailModel)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/email/receive/update/email?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Внимание, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Подтвердите, что это вы хотите поменять email с устройства {emailModel.UserPlatforms}. Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Смена", "почты", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        public async Task SendConfirmNewEmail(DataMailLink emailModel)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/email/receive/new?" +
                $"token={emailModel.EmailToken}";
            string patternBody = $"<div style=\"font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word\"><div style=\"font-size:22px; color: #BDEED6;\">Дорогой, {emailModel.UserName}</div></div><div style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color: #BDEED6;\"><div style=\"color: #BDEED6;\">Подтвердите, что это ваш новый email. Отправка с устройства {emailModel.UserPlatforms}. Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, вас автоматически отключит со всех устройств</div><div style=\"color: #BDEED6;\">С уважением команда InCase</div><div style=\"padding-top:50px;text-align:center\"><a href=\"{uri}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-weight: bold; padding: 10px 75px; font-size: 16px; color: #BDEED6; border: 2px solid #BDEED6; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">Подтвердить</a></div></div>";
            string body = CreateEmailTemplate("Смена", "почты", patternBody);

            await SendToEmail(emailModel.UserEmail, subject, body);
        }

        private async Task SendToEmail(string email, string subject, string body)
        {
            using var client = new SmtpClient();
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", _smtpEmail));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = body };


                await client.ConnectAsync(_host, _port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_smtpEmail, _smtpPassword);
                await client.SendAsync(emailMessage);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
