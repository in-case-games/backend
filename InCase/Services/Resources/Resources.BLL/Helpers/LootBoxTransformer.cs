using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class LootBoxTransformer
    {
        public static LootBoxResponse ToResponse(this LootBox box) =>
            new()
            {
                Id = box.Id,
                Cost = box.Cost,
                Name = box.Name,
                HashName = box.HashName,
            };

        public static List<LootBoxResponse> ToResponse(this IEnumerable<LootBox> boxes)
        {
            List<LootBoxResponse> response = new();

            foreach (var box in boxes)
                response.Add(ToResponse(box));

            return response;
        }

        public static LootBox ToEntity(this LootBoxRequest request, bool isNewGuid = false) =>
            new()
            {
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                GameId = request.GameId,
                Name = request.Name,
                HashName = request.HashName,
            };
    }
}
