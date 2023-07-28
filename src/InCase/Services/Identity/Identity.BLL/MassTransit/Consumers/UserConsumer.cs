using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.DAL.Entities;
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
            var template = context.Message;

            User? user = await _userService.GetByConsumerAsync(template.Id);

            if (user is null)
                await _userService.CreateAsync(template);
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
            else if (user.Login != template.Login)
                await _userService.UpdateLoginAsync(template.ToRequest());
        }
    }
}
