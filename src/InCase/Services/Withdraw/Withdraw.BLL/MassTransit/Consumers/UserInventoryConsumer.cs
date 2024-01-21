using Infrastructure.MassTransit.User;
using MassTransit;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.MassTransit.Consumers;

public class UserInventoryConsumer(IUserInventoryService inventoryService) : IConsumer<UserInventoryTemplate>
{
    public async Task Consume(ConsumeContext<UserInventoryTemplate> context)
    {
        var inventory = await inventoryService.GetByConsumerAsync(context.Message.Id);

        if(inventory is null) await inventoryService.CreateAsync(context.Message);
    }
}