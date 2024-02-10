using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;
public class GameMarket : BaseEntity
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [JsonIgnore]
    public Guid GameId { get; set; }
    [JsonIgnore]
    public Game? Game { get; set; }
    [JsonIgnore]
    public IEnumerable<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
}