using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxInventoryResponse : BaseEntity
    {
        public LootBoxResponse? Box { get; set; }
        public GameItemResponse? Item { get; set; }
    }
}
