using EmailSender.BLL.Interfaces;
using Infrastructure.MassTransit.Email;
using MassTransit;

namespace EmailSender.BLL.MassTransit.Consumers;

public class EmailConsumer(IUserService userService, IEmailService emailService) : IConsumer<EmailTemplate>
{
    public async Task Consume(ConsumeContext<EmailTemplate> context)
    {
        var user = await userService.GetAsync(context.Message.Email);

        if (user is null || user.AdditionalInfo!.IsNotifyEmail || context.Message.IsRequiredMessage)
            await emailService.SendToEmailAsync(context.Message);
    }
}