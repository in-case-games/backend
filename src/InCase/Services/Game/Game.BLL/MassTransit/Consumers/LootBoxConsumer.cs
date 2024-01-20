using Game.BLL.Interfaces;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxConsumer(ILootBoxService boxService) : IConsumer<LootBoxTemplate>
    {
        public async Task Consume(ConsumeContext<LootBoxTemplate> context)
        {
            var box = await boxService.GetAsync(context.Message.Id);

            if (box is null) await boxService.CreateAsync(context.Message);
            else if (context.Message.IsDeleted) await boxService.DeleteAsync(context.Message.Id);
            else await boxService.UpdateAsync(context.Message);
        }
    }
}
