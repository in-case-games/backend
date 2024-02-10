using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models;
public class ExchangeItemModel : BaseEntity
{
    public Guid ItemId { get; set; }
    public int Count { get; set;}
}