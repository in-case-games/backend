using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models
{
    public class UserHistoryWithdrawResponse : BaseEntity
    {
        public string? InvoiceId { get; set; }
        public decimal FixedCost { get; set; }
        public string? Status { get; set; }
        public DateTime Date { get; set; }
        public Guid ItemId { get; set; }
        public Guid MarketId { get; set; }
    }
}
