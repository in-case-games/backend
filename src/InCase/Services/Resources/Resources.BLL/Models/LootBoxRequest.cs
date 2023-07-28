using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxRequest : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsLocked { get; set; } = true;
        public Guid GameId { get; set; }
    }
}
