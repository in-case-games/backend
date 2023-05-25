using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class LootBoxResponse : BaseEntity
    {
        public string? Name { get; set; }
        public string? HashName { get; set; }
        public decimal Cost { get; set; }
    }
}
