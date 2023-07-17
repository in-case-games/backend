using Payment.DAL.Entities;

namespace Payment.BLL.Models
{
    public class UserPromocodeRequest : BaseEntity
    {
        public decimal Discount { get; set; }
        public Guid UserId { get; set; }
    }
}
