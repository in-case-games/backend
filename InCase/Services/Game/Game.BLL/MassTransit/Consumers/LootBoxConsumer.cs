using Game.BLL.Interfaces;
using Game.DAL.Entities;
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
            var template = context.Message;

            LootBox? box = await _boxService.GetAsync(template.Id);

            if (box is null)
                await _boxService.CreateAsync(template);
            else if (template.IsDeleted)
                await _boxService.DeleteAsync(template.Id);
            else
                await _boxService.UpdateAsync(template);
        }
    }
}
