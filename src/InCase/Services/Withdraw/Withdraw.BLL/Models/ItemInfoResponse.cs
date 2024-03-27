using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models;
public class ItemInfoResponse
{
    public string? Id { get; set; }
    public int PriceKopecks { get; set; }
    public int Count { get; set; }
    public GameItem Item { get; set; } = null!;
    public GameMarket Market { get; set; } = null!;
}