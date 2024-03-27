using Infrastructure.MassTransit.Email;

namespace EmailSender.BLL.Interfaces;
public interface IEmailService
{
    public Task SendToEmailAsync(EmailTemplate template, CancellationToken cancellationToken = default);
}