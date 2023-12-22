using Infrastructure.MassTransit.User;
using MassTransit;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.MassTransit.Consumers
{
    public class UserInventoryConsumer : IConsumer<UserInventoryTemplate>
    {
        private readonly IUserInventoryService _inventoryService;

        public UserInventoryConsumer(IUserInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task Consume(ConsumeContext<UserInventoryTemplate> context)
        {
            var inventory = await _inventoryService.GetByConsumerAsync(context.Message.Id);

            if(inventory is null) await _inventoryService.CreateAsync(context.Message);
        }
    }
}
