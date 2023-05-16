using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class UserPayments : BaseEntity
    {
        public string? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public string? Status { get; set; }
        public Guid UserId { get; set; }
    }
}
