using Game.BLL.Exceptions;
using Game.DAL.Data;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers;
public class UserInventoryBackConsumer(ApplicationDbContext context) : IConsumer<UserInventoryBackTemplate>
{
    public async Task Consume(ConsumeContext<UserInventoryBackTemplate> context1)
    {
        var info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == context1.Message.UserId) ??
            throw new NotFoundException("Пользователь не найден");

        info.Balance += context1.Message.FixedCost;

        await context.SaveChangesAsync();
    }
}