using Identity.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Identity.BLL.MassTransit.Consumers;

public class UserConsumer(IUserService userService) : IConsumer<UserTemplate>
{
    public async Task Consume(ConsumeContext<UserTemplate> context)
    {
        var user = await userService.GetByConsumerAsync(context.Message.Id);

        if (user is null) await userService.CreateAsync(context.Message);
        else if (context.Message.IsDeleted) await userService.DeleteAsync(user.Id);
        else if (user.Login != context.Message.Login) await userService.UpdateLoginAsync(context.Message);
    }
}