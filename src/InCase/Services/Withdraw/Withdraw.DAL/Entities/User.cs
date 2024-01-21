using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;

public class User : BaseEntity
{
    [JsonIgnore]
    public IEnumerable<UserInventory>? Inventories { get; set; }
    [JsonIgnore]
    public IEnumerable<UserHistoryWithdraw>? HistoriesWithdraws { get; set; }
}