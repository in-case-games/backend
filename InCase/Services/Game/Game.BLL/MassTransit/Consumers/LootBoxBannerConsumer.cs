using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxBannerConsumer : IConsumer<LootBoxBannerTemplate>
    {
        private readonly ILootBoxService _boxService;
        private readonly ApplicationDbContext _context;

        public LootBoxBannerConsumer(
            ILootBoxService boxService,
            ApplicationDbContext context)
        {
            _boxService = boxService;
            _context = context;
        }

        public async Task Consume(ConsumeContext<LootBoxBannerTemplate> context)
        {
            var template = context.Message;

            LootBox? box = await _context.Boxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == template.BoxId);

            if (box is not null)
                await _boxService.UpdateExpirationBannerAsync(template);
        }
    }
}
