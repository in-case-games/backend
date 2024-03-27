using Game.BLL.Interfaces;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers;
public class LootBoxBannerConsumer(ILootBoxService boxService) : IConsumer<LootBoxBannerTemplate>
{
    public async Task Consume(ConsumeContext<LootBoxBannerTemplate> context)
    {
        var box = await boxService.GetAsync(context.Message.BoxId);

        if (box is not null)
        {
            context.Message.ExpirationDate = context.Message.IsDeleted ? null : context.Message.ExpirationDate;
            await boxService.UpdateExpirationBannerAsync(context.Message);
        }
    }
}