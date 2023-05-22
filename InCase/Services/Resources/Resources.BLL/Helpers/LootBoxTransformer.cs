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
                Inventories = box.Inventories?.ToResponse(),
            };

        public static List<LootBoxResponse> ToResponse(this IEnumerable<LootBox> boxes)
        {
            List<LootBoxResponse> response = new();

            foreach (var box in boxes)
                response.Add(ToResponse(box));

            return response;
        }
    }
}
