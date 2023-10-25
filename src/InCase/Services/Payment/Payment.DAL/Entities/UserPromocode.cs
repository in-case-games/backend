using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class UserPromocode : BaseEntity
    {
        public decimal Discount { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
