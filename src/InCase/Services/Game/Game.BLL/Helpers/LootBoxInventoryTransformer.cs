using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Helpers
{
    public static class LootBoxInventoryTransformer
    {
        public static LootBoxInventory ToEntity(this LootBoxInventoryTemplate template) => new()
        {
            Id = template.Id,
            BoxId = template.BoxId,
            ChanceWining = template.ChanceWining,
            ItemId = template.ItemId,
        };
    }
}
