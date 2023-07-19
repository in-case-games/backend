using Infrastructure.MassTransit.Resources;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.MassTransit.Consumers
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

            GameItem? item = await _context.GameItems
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
