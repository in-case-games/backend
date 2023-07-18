using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxInventoryConsumer : IConsumer<LootBoxInventoryTemplate>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILootBoxInventoryService _inventoryService;

        public LootBoxInventoryConsumer(
            ApplicationDbContext context,
            ILootBoxInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public async Task Consume(ConsumeContext<LootBoxInventoryTemplate> context)
        {
            var template = context.Message;

            LootBoxInventory? inventory = await _context.BoxInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.Id == template.Id);

            if (inventory is null)
                await _inventoryService.CreateAsync(template);
            else if (template.IsDeleted)
                await _inventoryService.DeleteAsync(template.Id);
            else
                await _inventoryService.UpdateAsync(template);
        }
    }
}
