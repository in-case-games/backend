using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IEmailService
    {
        public Task SendToEmail(string email, string subject, EmailTemplate template);
    }
}
