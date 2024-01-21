using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities;

public class UserPromocode : BaseEntity
{
    public DateTime Date { get; set; }
    public bool IsActivated { get; set; } = false;
    public PromocodeEntity? Promocode { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid PromocodeId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}