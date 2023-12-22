using Game.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class UserPromocodeConsumer : IConsumer<UserPromocodeTemplate>
    {
        private readonly IUserPromocodeService _promocodeService;

        public UserPromocodeConsumer(IUserPromocodeService promocodeService)
        {
            _promocodeService = promocodeService;
        }

        public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
        {
            if (context.Message.Type?.Name == "box")
            {
                var promo = await _promocodeService.GetAsync(context.Message.Id);

                if (promo is null) await _promocodeService.CreateAsync(context.Message);
                else await _promocodeService.UpdateAsync(context.Message);
            }
        }
    }
}
