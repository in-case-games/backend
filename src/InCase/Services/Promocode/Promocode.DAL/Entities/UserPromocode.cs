using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities;
public class UserPromoCode : BaseEntity
{
    public DateTime Date { get; set; }
    public bool IsActivated { get; set; } = false;
    public PromoCode? PromoCode { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid PromoCodeId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}