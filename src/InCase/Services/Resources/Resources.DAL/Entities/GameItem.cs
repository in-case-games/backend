using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Resources.DAL.Entities;
public class GameItem : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(200)]
    public string? HashName { get; set; }
    [MaxLength(200)]
    public string? IdForMarket { get; set; }
    public decimal Cost { get; set; }
    public DateTime UpdatedIn { get; set; } = DateTime.UtcNow;
    public DateTime UpdateTo { get; set; } = DateTime.UtcNow;

    public GameItemQuality? Quality { get; set; }
    public GameItemType? Type { get; set; }
    public GameItemRarity? Rarity { get; set; }

    [JsonIgnore]
    public Guid GameId { get; set; }
    [JsonIgnore]
    public Guid TypeId { get; set; }
    [JsonIgnore]
    public Guid RarityId { get; set; }
    [JsonIgnore]
    public Guid QualityId { get; set; }
    [JsonIgnore]
    public Game? Game { get; set; }
    [JsonIgnore]
    public IEnumerable<LootBoxInventory>? Inventories { get; set; }
}