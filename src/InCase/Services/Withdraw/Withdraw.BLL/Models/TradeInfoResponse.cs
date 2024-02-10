using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models;
public class TradeInfoResponse
{
    public string Id { get; set; } = null!;
    public string Status { get; set; } = null!;
    public GameItem? Item { get; set; }
}