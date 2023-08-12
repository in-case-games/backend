using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.MassTransit.Consumer
{
    public class LootBoxBackConsumer : IConsumer<LootBoxBackTemplate>
    {
        private readonly ApplicationDbContext _context;

        public LootBoxBackConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<LootBoxBackTemplate> context)
        {
            var template = context.Message;

            LootBox? box = await _context.LootBoxes
                .FirstOrDefaultAsync(lb => lb.Id == template.Id);

            if (box is not null)
            {
                box.IsLocked = template.IsLocked;
                box.Cost = template.Cost;

                await _context.SaveChangesAsync();
            }
        }
    }
}
