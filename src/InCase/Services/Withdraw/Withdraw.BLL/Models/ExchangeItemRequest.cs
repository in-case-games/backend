using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models;

public class ExchangeItemRequest : BaseEntity
{
    public Guid InventoryId { get; set; }
    public ICollection<ExchangeItemModel>? Items { get; set; }
}