using Infrastructure.MassTransit.User;
using MassTransit;
using Support.BLL.Interfaces;

namespace Support.BLL.MassTransit.Consumers
{
    public class UserConsumer(IUserService userService) : IConsumer<UserTemplate>
    {
        public async Task Consume(ConsumeContext<UserTemplate> context)
        {
            var user = await userService.GetAsync(context.Message.Id);

            if (user is null) await userService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await userService.DeleteAsync(user.Id);
        }
    }
}
