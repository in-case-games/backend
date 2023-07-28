using Game.BLL.Interfaces;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class GameItemConsumer : IConsumer<GameItemTemplate>
    {
        private readonly IGameItemService _itemService;

        public GameItemConsumer(IGameItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task Consume(ConsumeContext<GameItemTemplate> context)
        {
            var template = context.Message;

            GameItem? item = await _itemService.GetAsync(template.Id);

            if (item is null)
                await _itemService.CreateAsync(template);
            else if (template.IsDeleted)
                await _itemService.DeleteAsync(template.Id);
            else
                await _itemService.UpdateAsync(template);
        }
    }
}
