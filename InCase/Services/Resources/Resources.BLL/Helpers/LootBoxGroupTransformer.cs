using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
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

        public static List<LootBoxGroupResponse> ToResponse(this List<LootBoxGroup> groups)
        {
            List<LootBoxGroupResponse> response = new();

            foreach (var group in groups)
                response.Add(ToResponse(group));

            return response;
        }

        public static LootBoxGroup ToEntity(
            this LootBoxGroupRequest request,
            bool isNewGuid = false) =>
            new()
            {
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                BoxId = request.BoxId,
                GameId = request.GameId,
                GroupId = request.GroupId,
            };
    }
}
