using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxGroupResponse : BaseEntity
    {
        public GroupLootBox? Group { get; set; }
        public LootBox? Box { get; set; }
        public Game? Game { get; set; }
    }
}
