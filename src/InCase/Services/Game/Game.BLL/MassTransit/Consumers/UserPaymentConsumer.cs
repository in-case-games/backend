using Game.BLL.Exceptions;
using Game.DAL.Data;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers;
public class UserPaymentConsumer(ApplicationDbContext context) : IConsumer<UserPaymentTemplate>
{
    public async Task Consume(ConsumeContext<UserPaymentTemplate> context1)
    {
        var info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == context1.Message.UserId) ??
            throw new NotFoundException("Пользователь не найден");

        info.Balance += context1.Message.Amount;

        await context.SaveChangesAsync();
    }
}