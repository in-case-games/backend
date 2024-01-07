using Identity.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Identity.BLL.MassTransit.Consumers
{
    public class UserConsumer : IConsumer<UserTemplate>
    {
        private readonly IUserService _userService;

        public UserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserTemplate> context)
        {
            var user = await _userService.GetByConsumerAsync(context.Message.Id);

            if (user is null) await _userService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _userService.DeleteAsync(user.Id);
            else if (user.Login != context.Message.Login) await _userService.UpdateLoginAsync(context.Message);
        }
    }
}
