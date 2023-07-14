using EmailSender.BLL.Interfaces;
using EmailSender.BLL.MassTransit.Models;
using EmailSender.DAL.Data;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailSender.BLL.Services
{
    public class EmailService : IEmailService
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

        public async Task SendToEmailAsync(EmailTemplate template)
        {
            using var client = new SmtpClient();

            string body = CreateBodyLetter(template);

            string email = template.Email;
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _smtpEmail));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = template.Subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            await client.ConnectAsync(_host, _port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_smtpEmail, _smtpPassword);

            try
            {
                await client.SendAsync(emailMessage);
            }
            catch (SmtpCommandException) { }
            catch (Exception) { }
        }

        private static string ConvertToBodyTemplate(EmailTemplate template)
        {
            string buttonTemplate = "";
            string bannerTemplate = "";
            string bannerTableTemplates = "";

            template.Body.Description += "<br>С уважением команда InCase.";

            if (string.IsNullOrEmpty(template.Body.ButtonText))
                template.Body.ButtonText = "Подтверждаю";

            if (!string.IsNullOrEmpty(template.Body.ButtonLink))
                buttonTemplate = $"<div style=\"padding-bottom:30px;text-align:center\"><a href=\"{template.Body.ButtonLink}\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; background-color: transparent; font-family: 'Trebuchet MS',sans-serif; font-weight: bold; padding: 8px 85px; font-size: 16px; color: #FD7E21; border: 2px solid #FD7E21; border-radius: 8px;\" target=\"_blank\" data-saferedirecturl=\"ya.ru\">{template.Body.ButtonText}</a></div>";

            foreach (var banner in template.BannerTemplates)
                bannerTableTemplates += $"<table align=\"center\" style=\"padding:10px;\"><tbody><tr><td><a href=\"{banner.Href}\"><img align=\"center\" src=\"{banner.ImageUri}\" width=\"350\" height=\"110\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td></tr></tbody></table>";

            if (!string.IsNullOrEmpty(bannerTableTemplates))
                bannerTemplate = $"<div style=\"text-align:left;\"><hr style=\"border: 1px solid #FD7E21; margin: 10px 50px;\">{bannerTableTemplates}</div>";

            return $"<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'><html><head><meta http-equiv='Content-type' content='text/html; charset=utf-8'/></head><body><div style=\"margin:0;color: #FD7E21;padding:0\" bgcolor=\"#FFFFFF\"><table width=\"100%\" height=\"100%\" style=\"min-width:480px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" lang=\"en\"><tbody><tr height=\"32\" style=\"height:32px\"><td></td></tr><tr align=\"center\"><td><div><div></div></div><table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"padding-bottom:10px;max-width:520px;min-width:480px;color: #FD7E21;\" bgcolor=\"#1A1A1D\"><tbody><tr><td width=\"8\" style=\"width:8px\"></td><td><div style=\"padding:40px 10px 0px 10px\" align=\"center\"><!--Header--><div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;line-height:32px;padding-bottom:60px;text-align:center;word-break:break-word\"><table align=\"center\"><tbody><tr style=\"line-height:normal;\"><td align=\"left\"><p style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;font-size:20px;line-height:20px;margin: 0;color: #FD7E21;\">{template.Header.Title}</p><p style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;font-size:20px;line-height:20px;margin: 0 0 0 50px;color: #FD7E21;\">{template.Header.Subtitle}</p></td><td width=\"160px\"></td><td align=\"right\"><a href=\"https://yandex.ru\" style=\"text-decoration: none;font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;font-size:40px;line-height:20px;margin: 0;color: #FD7E21;cursor: pointer;\">InCase</a></td></tr></tbody></table></div><!--Body--><div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;line-height:32px;padding-bottom:18px;text-align:center;word-break:break-word;font-size:20px;color:#fd7e21;\">{template.Body.Title}</div><div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;font-size:15px;line-height:20px;text-align:center;color: #FD7E21;padding-bottom:30px;\">{template.Body.Description}</div>{buttonTemplate}{bannerTemplate}<!--FOOTER--><div style=\"text-align:left;\"><hr style=\"border: 1px solid #FD7E21; margin: 10px 50px;\"><table align=\"center\" style=\"padding-bottom:20px;\"><tbody><tr><td><img align=\"center\" src=\"https://sun9-4.userapi.com/impg/rzB56cbWicyBWzmj5u0f_BZ4-IIJFguSnFrPcw/mIziSoNmxvk.jpg?size=175x220&quality=96&sign=46ea7dfbe656fa8ddeadd1896e6b93f4&type=album\" width=\"100\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></td></tr></tbody></table><div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;font-size:14px;color: #FD7E21;line-height:18px;text-align:center\"><table align=\"center\" style=\"padding-bottom:20px;\"><tbody><tr><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;margin-right: 10px;\" src=\"https://sun9-33.userapi.com/impg/pDHT-ZNEx2eALiBOCavVLivuxZoJcCBbyRukwA/pQiJz3xs1HQ.jpg?size=50x29&quality=96&sign=b32c387df9355b766f711a031c36b443&type=album\" width=\"45\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;margin-right: 10px;\" src=\"https://sun9-21.userapi.com/impg/A-6P4uCJQZfR66QrTrieXWApphA63QRebdH9hw/7DWAmoZcQY8.jpg?size=37x37&quality=96&sign=464dd75123df66fee21ec449826783bb&type=album\" href=\"yandex.ru\" width=\"32\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td><td><a href=\"https://yandex.ru\"><img style=\"cursor: pointer;margin-right: 10px;\" src=\"https://sun9-18.userapi.com/impg/QHdJOSjvwnI5RF242kIkz8ANmj7e0sCOxL1MxA/gV-DPmqRvB0.jpg?size=38x38&quality=96&sign=5d32284e755ed8a955a90c98751d168f&type=album\" width=\"32\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td></tr></tbody></table></div></div></div></td><td width=\"8\" style=\"width:8px\"></td></tr></tbody></table></td></tr><tr height=\"32\" style=\"height:32px\"><td></td></tr></tbody></table></div></body></html>";
        }

        private string CreateBodyLetter(EmailTemplate template)
        {
            if (!string.IsNullOrEmpty(template.Body.ButtonLink))
                template.Body.ButtonLink = _requestUrl + template.Body.ButtonLink;

            if (string.IsNullOrEmpty(template.Header.Title))
            {
                List<string> headerWords = template.Subject.Split(" ").ToList();

                template.Header.Title = headerWords[0];
                headerWords.Remove(headerWords[0]);

                if (headerWords.Count >= 2)
                {
                    template.Header.Title += " " + headerWords[0];
                    headerWords.Remove(headerWords[0]);
                }

                foreach (var word in headerWords)
                    template.Header.Subtitle += word + " ";
            }

            return ConvertToBodyTemplate(template);
        }
    }
}
