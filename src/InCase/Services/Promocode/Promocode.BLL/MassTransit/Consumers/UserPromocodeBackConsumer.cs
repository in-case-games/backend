using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Promocode.DAL.Data;

namespace Promocode.BLL.MassTransit.Consumers;

public class UserPromocodeBackConsumer(ApplicationDbContext context) : IConsumer<UserPromocodeBackTemplate>
{
    public async Task Consume(ConsumeContext<UserPromocodeBackTemplate> context1)
    {
        var promocode = await context.UserPromocodes.FirstOrDefaultAsync(ur => ur.Id == context1.Message.Id);

        if(promocode is not null)
        {
            promocode.IsActivated = true;

            await context.SaveChangesAsync();
        }
    }
}