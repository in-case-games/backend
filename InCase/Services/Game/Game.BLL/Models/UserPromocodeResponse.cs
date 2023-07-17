using Game.DAL.Entities;

namespace Game.BLL.Models
{
    public class UserPromocodeResponse : BaseEntity
    {
        public decimal Discount { get; set; }
        public Guid UserId { get; set; }
    }
}
