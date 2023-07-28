using Infrastructure.MassTransit.Resources;
using MassTransit;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.MassTransit.Consumers
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
            GameItemTemplate template = context.Message;
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
