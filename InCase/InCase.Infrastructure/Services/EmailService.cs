using InCase.Domain.Entities.Email;
using InCase.Infrastructure.Utils;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> SendNotifyToEmail(string email, string subject, EmailTemplate emailTemplate)
        {
            //TODO
            string body = emailTemplate.CreateEmailTemplate();
            try
            {
                await SendToEmail(email, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch(Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendSignUp(DataMailLink data)
        {
            string subject = "Подтверждение регистрации.";
            string uri = $"{_requestUrl}api/email/confirm?token={data.EmailToken}";
            string body = data.CreateSignUpTemplate(uri);
            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendSignIn(DataMailLink data)
        {
            string subject = "Подтверждение входа.";
            string uri = $"{_requestUrl}api/email/confirm/account?token={data.EmailToken}";
            string body = data.CreateSignInTemplate(uri);
            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendSuccessVerifedAccount(DataMailLink data)
        {
            string subject = "Подтверждение входа.";
            string body = data.CreateSuccessVerifedAccountTemplate();

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendLoginAttempt(DataMailLink data)
        {
            string subject = "Вход в аккаунт.";
            string body = data.CreateLoginAttemptTemplate();

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendDeleteAccount(DataMailLink data)
        {
            string subject = "Подтвердите удаление аккаунта";
            string uri = $"{_requestUrl}/email/confirm/delete?token={data.EmailToken}";
            string body = data.CreateDeleteAccountTemplate(uri);

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendChangePassword(DataMailLink data)
        {
            string subject = "Подтвердите изменение пароля";
            string uri = $"{_requestUrl}/email/confirm/update/password?token={data.EmailToken}";
            string body = data.CreateChangePasswordTemplate(uri);

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendChangeEmail(DataMailLink data)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/email/confirm/update/email?token={data.EmailToken}";
            string body = data.CreateChangeEmailTemplate(uri);

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public async Task<IActionResult> SendConfirmNewEmail(DataMailLink data)
        {
            string subject = "Подтвердите изменение почты";
            string uri = $"{_requestUrl}/email/confirm/new?token={data.EmailToken}";
            string body = data.CreateConfirmNewEmailTemplate(uri);

            try
            {
                await SendToEmail(data.UserEmail, subject, body);
                return ResponseUtil.SendEmail();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
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
