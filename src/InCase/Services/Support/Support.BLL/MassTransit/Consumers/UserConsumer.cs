using Infrastructure.MassTransit.User;
using MassTransit;
using Support.BLL.Interfaces;
using Support.DAL.Entities;

namespace Support.BLL.MassTransit.Consumers
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

            User? user = await _userService.GetAsync(template.Id);

            if (user is null)
                await _userService.CreateAsync(template);
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
        }
    }
}
