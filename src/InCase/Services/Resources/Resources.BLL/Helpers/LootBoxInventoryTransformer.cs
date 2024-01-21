using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers;

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

    public static List<LootBoxInventoryResponse> ToResponse(this List<LootBoxInventory> inventories) =>
        inventories.Select(ToResponse).ToList();
}