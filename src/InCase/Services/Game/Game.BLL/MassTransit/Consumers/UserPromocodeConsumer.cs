using Game.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers;

public class UserPromocodeConsumer(IUserPromocodeService promocodeService) : IConsumer<UserPromocodeTemplate>
{
    public async Task Consume(ConsumeContext<UserPromocodeTemplate> context)
    {
        if (context.Message.Type?.Name == "box")
        {
            var promo = await promocodeService.GetAsync(context.Message.UserId);

            if (promo is null) await promocodeService.CreateAsync(context.Message);
            else await promocodeService.UpdateAsync(context.Message);
        }
    }
}