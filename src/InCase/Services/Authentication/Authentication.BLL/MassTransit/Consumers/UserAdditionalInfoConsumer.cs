using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Identity.BLL.MassTransit.Consumers
{
    public class UserAdditionalInfoConsumer : IConsumer<UserAdditionalInfoTemplate>
    {
        private readonly IUserAdditionalInfoService _infoService;

        public UserAdditionalInfoConsumer(IUserAdditionalInfoService infoService)
        {
            _infoService = infoService;
        }

        public async Task Consume(ConsumeContext<UserAdditionalInfoTemplate> context)
        {
            var template = context.Message;
            
            await _infoService.UpdateAsync(template);
        }
    }
}
