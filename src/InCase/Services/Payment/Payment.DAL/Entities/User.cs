using System.Text.Json.Serialization;

namespace Payment.DAL.Entities;

public class User : BaseEntity
{
    [JsonIgnore]
    public IEnumerable<UserPayment>? Payments { get; set; }
    [JsonIgnore]
    public UserPromocode? Promocode { get; set; }
}