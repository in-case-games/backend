using Infrastructure.MassTransit.User;
using MassTransit;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Entities;

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
            var template = context.Message;

            UserInventory? inventory = await _inventoryService.GetByConsumerAsync(template.Id);

            if(inventory is null)
                await _inventoryService.CreateAsync(template);
        }
    }
}
