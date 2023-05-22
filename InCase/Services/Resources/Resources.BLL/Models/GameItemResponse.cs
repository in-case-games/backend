using Resources.DAL.Entities;

namespace Resources.BLL.Models
{
    public class GameItemResponse : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public string? Quality { get; set; }
        public string? Type { get; set; }
        public string? Rarity { get; set; }
    }
}
