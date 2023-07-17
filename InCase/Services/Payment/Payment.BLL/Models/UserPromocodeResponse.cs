using Payment.DAL.Entities;

namespace Payment.BLL.Models
{
    public class UserPromocodeResponse : BaseEntity
    {
        public decimal Discount { get; set; }
        public Guid UserId { get; set; }
    }
}
