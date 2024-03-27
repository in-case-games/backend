using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;
public class GameItem : BaseEntity
{
    [MaxLength(100)]
    public string? IdForMarket { get; set; }
    public decimal Cost { get; set; }

    [JsonIgnore]
    public Guid GameId { get; set; }
    [JsonIgnore]
    public Game? Game { get; set; }

    [JsonIgnore]
    public IEnumerable<UserHistoryWithdraw>? HistoriesWithdraws { get; set; }
    [JsonIgnore]
    public IEnumerable<UserInventory>? Inventories { get; set; }
}