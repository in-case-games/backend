using Infrastructure.MassTransit.Resources;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class LootBoxInventoryTransformer
    {
        public static LootBoxInventoryResponse ToResponse(this LootBoxInventory inventory) =>
            new()
            {
                Id = inventory.Id,
                Item = inventory.Item?.ToResponse(),
                Box = inventory.Box?.ToResponse(),
                ChanceWining = inventory.ChanceWining
            };

        public static List<LootBoxInventoryResponse> ToResponse(
            this List<LootBoxInventory> inventories)
        {
            List<LootBoxInventoryResponse> response = new();

            foreach (var inventory in inventories)
                response.Add(ToResponse(inventory));

            return response;
        }

        public static LootBoxInventory ToEntity(
            this LootBoxInventoryRequest request, bool isNewGuid = false) =>
            new()
            {
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                BoxId = request.BoxId,
                ItemId = request.ItemId,
                ChanceWining = request.ChanceWining
            };

        public static LootBoxInventoryTemplate ToTemplate(
            this LootBoxInventoryRequest request, 
            bool isDeleted = false) => new()
        {
            Id = request.Id,
            BoxId = request.BoxId,
            ChanceWining = request.ChanceWining,
            ItemId = request.ItemId,
            IsDeleted = isDeleted
        };

        public static LootBoxInventoryTemplate ToTemplate(
            this LootBoxInventory entity,
            bool isDeleted = false) => new()
        {
            Id = entity.Id,
            BoxId = entity.BoxId,
            ChanceWining = entity.ChanceWining,
            ItemId = entity.ItemId,
            IsDeleted = isDeleted
        };
    }
}
