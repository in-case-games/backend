using Infrastructure.MassTransit.User;
using MassTransit;
using Payment.BLL.Interfaces;

namespace Payment.BLL.MassTransit.Consumers;

public class UserPromocodeConsumer(IUserPromocodeService promocodeService) : IConsumer<UserPromocodeTemplate>
{
    public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
    {
        if (context.Message.Type?.Name == "balance")
        {
            var userPromocode = await promocodeService.GetAsync(context.Message.Id, context.Message.UserId);

            if (userPromocode is null) await promocodeService.CreateAsync(context.Message);
            else await promocodeService.UpdateAsync(context.Message);
        }
    }
}