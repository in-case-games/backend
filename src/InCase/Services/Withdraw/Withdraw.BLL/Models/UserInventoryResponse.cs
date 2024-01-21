using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models;

public class UserInventoryResponse : BaseEntity
{
    public DateTime Date { get; set; }
    public decimal FixedCost { get; set; }
    public Guid ItemId { get; set; }
}