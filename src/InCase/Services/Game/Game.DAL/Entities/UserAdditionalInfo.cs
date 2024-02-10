using System.Text.Json.Serialization;

namespace Game.DAL.Entities;
public class UserAdditionalInfo : BaseEntity
{
    public decimal Balance { get; set; } = 0;
    public bool IsGuestMode { get; set; } = false;

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}