using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.MassTransit.Consumers
{
    public class GameItemConsumer : IConsumer<GameItemTemplate>
    {
        private readonly IGameItemService _itemService;
        private readonly ApplicationDbContext _context;

        public GameItemConsumer(
            IGameItemService itemService,
            ApplicationDbContext context)
        {
            _itemService = itemService;
            _context = context;
        }

        public async Task Consume(ConsumeContext<GameItemTemplate> context)
        {
            GameItemTemplate template = context.Message;

            GameItem? item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == template.Id);

            if (item is null)
                await _itemService.CreateAsync(template);
            else if (template.IsDeleted)
                await _itemService.DeleteAsync(template.Id);
            else
                await _itemService.UpdateAsync(template);
        }
    }
}
