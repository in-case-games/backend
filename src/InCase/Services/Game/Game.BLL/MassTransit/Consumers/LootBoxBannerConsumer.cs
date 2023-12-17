using Game.BLL.Interfaces;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers
{
    public class LootBoxBannerConsumer : IConsumer<LootBoxBannerTemplate>
    {
        private readonly ILootBoxService _boxService;

        public LootBoxBannerConsumer(ILootBoxService boxService)
        {
            _boxService = boxService;
        }

        public async Task Consume(ConsumeContext<LootBoxBannerTemplate> context)
        {
            var box = await _boxService.GetAsync(context.Message.BoxId);

            if (box is not null)
            {
                context.Message.ExpirationDate = context.Message.IsDeleted ? null : context.Message.ExpirationDate;
                await _boxService.UpdateExpirationBannerAsync(context.Message);        
            }
        }
    }
}
