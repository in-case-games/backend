namespace Withdraw.BLL.Models;

public class WithdrawItemRequest
{
    public Guid InventoryId { get; set; }
    public string? TradeUrl { get; set; }
}