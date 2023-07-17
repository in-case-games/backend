using Game.DAL.Entities;

namespace Game.BLL.Models
{
    public class UserPromocodeRequest : BaseEntity
    {
        public decimal Discount { get; set; }
        public Guid UserId { get; set; }
    }
}
