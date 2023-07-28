using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Entities;
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
            var template = context.Message;

            User? user = await _userService.GetAsync(template.Id);

            if (user is null)
                await _userService.CreateAsync(template);
            else if (template.IsDeleted)
                await _userService.DeleteAsync(user.Id);
            else if (user.Email != template.Email)
                await _userService.UpdateAsync(template);
        }
    }
}
