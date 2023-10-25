using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class GameResponse : BaseEntity
    {
        public string? Name { get; set; }

        public IEnumerable<LootBoxResponse>? Boxes { get; set; }
    }
}
