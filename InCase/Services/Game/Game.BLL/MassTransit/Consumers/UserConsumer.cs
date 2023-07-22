using Game.BLL.Interfaces;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
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
            UserTemplate template = context.Message;

            User? user = await _userService.GetAsync(template.Id);

            if (user is null)
                await _userService.CreateAsync(template);
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
        }
    }
}
