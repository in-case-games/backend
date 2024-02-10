using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Data;

namespace Promocode.BLL.MassTransit.Consumers;
public class UserPromoCodeBackConsumer(ApplicationDbContext context) : IConsumer<UserPromoCodeBackTemplate>
{
    public async Task Consume(ConsumeContext<UserPromoCodeBackTemplate> context1)
    {
        var promoCode = await context.UserPromoCodes.FirstOrDefaultAsync(ur => ur.Id == context1.Message.Id);

        if(promoCode is not null)
        {
            promoCode.IsActivated = true;

            await context.SaveChangesAsync();
        }
    }
}