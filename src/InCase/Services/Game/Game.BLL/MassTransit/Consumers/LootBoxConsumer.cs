using Game.BLL.Interfaces;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxConsumer : IConsumer<LootBoxTemplate>
    {
        private readonly ILootBoxService _boxService;

        public LootBoxConsumer(ILootBoxService boxService)
        {
            _boxService = boxService;
        }

        public async Task Consume(ConsumeContext<LootBoxTemplate> context)
        {
            var box = await _boxService.GetAsync(context.Message.Id);

            if (box is null) await _boxService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await _boxService.DeleteAsync(context.Message.Id);
            else await _boxService.UpdateAsync(context.Message);
        }
    }
}
