using Infrastructure.MassTransit.Resources;
using MassTransit;
using Withdraw.BLL.Interfaces;

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
            var item = await _itemService.GetAsync(context.Message.Id);

            if (item is null) await _itemService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _itemService.DeleteAsync(context.Message.Id);
            else await _itemService.UpdateAsync(context.Message);
        }
    }
}
