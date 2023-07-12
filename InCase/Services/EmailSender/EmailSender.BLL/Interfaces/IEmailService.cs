using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IEmailService
    {
        public Task SendToEmailAsync(string email, string subject, EmailTemplate template);
    }
}
