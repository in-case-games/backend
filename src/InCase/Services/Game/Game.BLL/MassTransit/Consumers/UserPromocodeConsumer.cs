using Game.BLL.Interfaces;
using Infrastructure.MassTransit.User;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers;
public class UserPromoCodeConsumer(IUserPromoCodeService promoCodeService) : IConsumer<UserPromoCodeTemplate>
{
    public async Task Consume(ConsumeContext<UserPromoCodeTemplate> context)
    {
        if (context.Message.Type?.Name == "box")
        {
            var promo = await promoCodeService.GetAsync(context.Message.UserId);

            if (promo is null) await promoCodeService.CreateAsync(context.Message);
            else await promoCodeService.UpdateAsync(context.Message);
        }
    }
}