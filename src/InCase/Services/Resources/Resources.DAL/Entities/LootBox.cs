using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Resources.DAL.Entities;
public class LootBox : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }
    public decimal Cost { get; set; }
    public bool IsLocked { get; set; }

    public IEnumerable<LootBoxInventory>? Inventories { get; set; }

    [JsonIgnore]
    public Guid GameId { get; set; }
    [JsonIgnore]
    public Game? Game { get; set; }
    [JsonIgnore]
    public LootBoxBanner? Banner { get; set; }
    [JsonIgnore]
    public IEnumerable<LootBoxGroup>? Groups { get; set; }
}