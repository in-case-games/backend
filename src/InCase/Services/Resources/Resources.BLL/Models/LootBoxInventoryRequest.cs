using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxInventoryRequest : BaseEntity
    {
        public int ChanceWining { get; set; }
        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }
    }
}
