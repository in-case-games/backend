using Infrastructure.MassTransit.User;
using MassTransit;
using Support.BLL.Interfaces;

namespace Support.BLL.MassTransit.Consumers;
public class UserConsumer(IUserService userService) : IConsumer<UserTemplate>
{
    public async Task Consume(ConsumeContext<UserTemplate> context)
    {
        var user = await userService.GetAsync(context.Message.Id);

        if (user is not null && context.Message.IsDeleted) await userService.DeleteAsync(user.Id);
        else if (user is null && !context.Message.IsDeleted) await userService.CreateAsync(context.Message);
    }
}