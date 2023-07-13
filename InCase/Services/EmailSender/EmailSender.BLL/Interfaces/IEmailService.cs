using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IEmailService
    {
        public Task SendToEmailAsync(EmailTemplate template);
    }
}
