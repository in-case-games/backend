using Game.BLL.Interfaces;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxInventoryConsumer : IConsumer<LootBoxInventoryTemplate>
    {
        private readonly ILootBoxInventoryService _inventoryService;

        public LootBoxInventoryConsumer(ILootBoxInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task Consume(ConsumeContext<LootBoxInventoryTemplate> context)
        {
            var inventory = await _inventoryService.GetAsync(context.Message.Id);

            if (inventory is null) await _inventoryService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _inventoryService.DeleteAsync(context.Message.Id);
            else await _inventoryService.UpdateAsync(context.Message);
        }
    }
}
