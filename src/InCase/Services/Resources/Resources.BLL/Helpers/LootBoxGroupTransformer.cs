using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers;
public static class LootBoxGroupTransformer
{
    public static LootBoxGroupResponse ToResponse(this LootBoxGroup group) =>
        new()
        {
            Id = group.Id,
            Box = group.Box?.ToResponse(),
            Game = group.Game?.ToResponse(),
            Group = group.Group,
        };

    public static List<LootBoxGroupResponse> ToResponse(this List<LootBoxGroup> groups) =>
        groups.Select(ToResponse).ToList();
}