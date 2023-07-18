using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxConsumer : IConsumer<LootBoxTemplate>
    {
        private readonly ILootBoxService _boxService;
        private readonly ApplicationDbContext _context;

        public LootBoxConsumer(
            ILootBoxService boxService,
            ApplicationDbContext context)
        {
            _boxService = boxService;
            _context = context;
        }

        public async Task Consume(ConsumeContext<LootBoxTemplate> context)
        {
            var template = context.Message;

            LootBox? box = await _context.Boxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == template.Id);

            if (box is null)
                await _boxService.CreateAsync(template);
            else if (template.IsDeleted)
                await _boxService.DeleteAsync(template.Id);
            else
                await _boxService.UpdateAsync(template);
        }
    }
}
