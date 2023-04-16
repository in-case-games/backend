using InCase.Domain.Entities.Email;
using InCase.Infrastructure.Utils;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Org.BouncyCastle.Crypto;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<IActionResult> SendToEmail(string email, string subject, EmailTemplate template)
        {
            using var client = new SmtpClient();
            
            string body = CreateBodyLetter(subject, template);

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

                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        private string CreateBodyLetter(string subject, EmailTemplate template)
        {
            if (!string.IsNullOrEmpty(template.BodyButtonLink))
                template.BodyButtonLink = _requestUrl + template.BodyButtonLink;

            if (string.IsNullOrEmpty(template.HeaderTitle))
            {
                List<string> headerWords = subject.Split(" ").ToList();

                template.HeaderTitle = headerWords[0];
                headerWords.Remove(headerWords[0]);

                if(headerWords.Count >= 2)
                {
                    template.HeaderTitle += " " + headerWords[0];
                    headerWords.Remove(headerWords[0]);
                }

                foreach (var word in headerWords)
                    template.HeaderSubtitle += word + " ";
            }

            return template.CreateEmailTemplate();
        }
    }
}
