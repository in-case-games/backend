using Infrastructure.MassTransit.User;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Helpers;
public static class UserInventoryTransformer
{
    public static UserInventoryResponse ToResponse(this UserInventory inventory) =>
        new()
        {
            Id = inventory.Id,
            Date = inventory.Date,
            FixedCost = inventory.FixedCost,
            ItemId = inventory.Item?.Id ?? inventory.ItemId,
        };

    public static List<UserInventoryResponse> ToResponse(this List<UserInventory> inventories) =>
        inventories.Select(ToResponse).ToList();

    public static UserInventoryBackTemplate ToTemplate(this UserInventory inventory) => 
        new()
        {
            Id = inventory.Id,
            FixedCost = inventory.FixedCost,
            UserId = inventory.UserId
        };
}