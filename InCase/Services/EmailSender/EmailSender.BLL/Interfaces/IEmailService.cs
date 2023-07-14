using EmailSender.BLL.MassTransit.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IEmailService
    {
        public Task SendToEmailAsync(EmailTemplate template);
    }
}
