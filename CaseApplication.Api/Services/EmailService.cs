using MailKit.Net.Smtp;
using MimeKit;

namespace CaseApplication.Api.Services
{
    public class EmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _smtpEmail;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _host = configuration["EmailConfig:Host"]!;
            _port = int.Parse(configuration["EmailConfig:Port"]!);
            _smtpEmail = configuration["EmailConfig:Email"]!;
            _smtpPassword = configuration["EmailConfig:Password"]!;
        }

        public async Task SendToEmail(string email, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
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
}
