using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Resources.DAL.Entities;
public class GameItemQuality : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public GameItem? Item { get; set; }
}