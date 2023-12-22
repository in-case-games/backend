using Authentication.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Authentication.BLL.MassTransit.Consumers
{
    public class UserAdditionalInfoConsumer : IConsumer<UserAdditionalInfoTemplate>
    {
        private readonly IUserAdditionalInfoService _infoService;

        public UserAdditionalInfoConsumer(IUserAdditionalInfoService infoService)
        {
            _infoService = infoService;
        }

        public async Task Consume(ConsumeContext<UserAdditionalInfoTemplate> context) => 
            await _infoService.UpdateAsync(context.Message);
    }
}
