using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.MassTransit.Consumer
{
    public class LootBoxLockedConsumer : IConsumer<LootBoxLockedTemplate>
    {
        private readonly ApplicationDbContext _context;

        public LootBoxLockedConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<LootBoxLockedTemplate> context)
        {
            var template = context.Message;

            LootBox? box = await _context.LootBoxes
                .FirstOrDefaultAsync(lb => lb.Id == template.Id);

            if(box is not null)
            {
                box.IsLocked = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
