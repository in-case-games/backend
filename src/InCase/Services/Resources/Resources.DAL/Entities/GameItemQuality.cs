using System.Text.Json.Serialization;

namespace Resources.DAL.Entities;

public class GameItemQuality : BaseEntity
{
    public string? Name { get; set; }

    [JsonIgnore]
    public GameItem? Item { get; set; }
}