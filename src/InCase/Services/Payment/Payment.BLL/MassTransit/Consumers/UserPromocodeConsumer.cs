using Infrastructure.MassTransit.User;
using MassTransit;
using Payment.BLL.Interfaces;

namespace Payment.BLL.MassTransit.Consumers
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
            if (context.Message.Type?.Name == "balance")
            {
                var userPromocode = await _promocodeService.GetAsync(context.Message.Id, context.Message.UserId);

                if (userPromocode is null) await _promocodeService.CreateAsync(context.Message);
                else await _promocodeService.UpdateAsync(context.Message);
            }
        }
    }
}
