using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;

public class UserHistoryWithdraw : BaseEntity
{
    public string? InvoiceId { get; set; }
    public string? TradeUrl { get; set; }
    public decimal FixedCost { get; set; }
    public DateTime Date { get; set; }
    public DateTime UpdateDate { get; set; }

    public GameItem? Item { get; set; }
    public GameMarket? Market { get; set; }
    public WithdrawStatus? Status { get; set; }

    [JsonIgnore]
    public Guid MarketId { get; set; }
    [JsonIgnore]
    public Guid StatusId { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid ItemId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}