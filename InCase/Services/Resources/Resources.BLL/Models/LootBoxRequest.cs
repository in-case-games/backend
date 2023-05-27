using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxRequest : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public Guid GameId { get; set; }
    }
}
