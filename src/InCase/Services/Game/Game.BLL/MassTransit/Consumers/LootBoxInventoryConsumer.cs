using Game.BLL.Interfaces;
using Game.DAL.Entities;
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
            var template = context.Message;

            LootBoxInventory? inventory = await _inventoryService.GetAsync(template.Id);

            if (inventory is null)
                await _inventoryService.CreateAsync(template);
            else if (template.IsDeleted)
                await _inventoryService.DeleteAsync(template.Id);
            else
                await _inventoryService.UpdateAsync(template);
        }
    }
}
