using Game.BLL.Interfaces;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class GameItemConsumer : IConsumer<GameItemTemplate>
    {
        private readonly IGameItemService _itemService;
        private readonly ILootBoxService _boxService;

        public GameItemConsumer(IGameItemService itemService, ILootBoxService boxService)
        {
            _itemService = itemService;
            _boxService = boxService;
        }

        public async Task Consume(ConsumeContext<GameItemTemplate> context)
        {
            var template = context.Message;

            GameItem item = await _itemService.GetAsync(template.Id) ??
                await _itemService.CreateAsync(template);

            if (template.IsDeleted)
                await _itemService.DeleteAsync(item.Id);
            else
                await _itemService.UpdateAsync(template);

            List<LootBox> boxes = await _boxService.CorrectCostAsync(item.Id);
            await _boxService.CorrectChancesAsync(boxes);   
        }
    }
}
