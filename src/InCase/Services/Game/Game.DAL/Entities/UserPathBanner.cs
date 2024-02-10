using System.Text.Json.Serialization;

namespace Game.DAL.Entities;
public class UserPathBanner : BaseEntity
{
    public int NumberSteps { get; set; }
    public decimal FixedCost { get; set; }

    public User? User { get; set; }
    public GameItem? Item { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid ItemId { get; set; }
    [JsonIgnore]
    public Guid BoxId { get; set; }
    [JsonIgnore]
    public LootBox? Box { get; set; }
}