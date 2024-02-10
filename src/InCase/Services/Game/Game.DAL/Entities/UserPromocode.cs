using System.Text.Json.Serialization;

namespace Game.DAL.Entities;
public class UserPromoCode : BaseEntity
{
    public decimal Discount { get; set; }

    public User? User { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}