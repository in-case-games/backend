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
            var template = context.Message;

            var box = await _boxService.GetAsync(template.BoxId);

            if (box is not null)
            {
                template.ExpirationDate = template.IsDeleted ? null : template.ExpirationDate;
                await _boxService.UpdateExpirationBannerAsync(template);
            }
        }
    }
}
