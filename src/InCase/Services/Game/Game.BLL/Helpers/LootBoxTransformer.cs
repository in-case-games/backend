using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Helpers
{
    public static class LootBoxTransformer
    {
        public static LootBox ToEntity(this LootBoxTemplate template) => new()
        {
            Id = template.Id,
            Cost = template.Cost,
            IsLocked = template.IsLocked,
        };
    }
}
