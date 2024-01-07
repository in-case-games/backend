using EmailSender.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace EmailSender.BLL.MassTransit.Consumers
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
            var user = await _userService.GetAsync(context.Message.Id);

            if (user is null) await _userService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _userService.DeleteAsync(user.Id);
            else if (user.Email != context.Message.Email) await _userService.UpdateAsync(context.Message);
        }
    }
}
