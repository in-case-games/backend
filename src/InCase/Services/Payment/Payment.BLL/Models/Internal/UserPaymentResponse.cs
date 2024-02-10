using Payment.DAL.Entities;

namespace Payment.BLL.Models.Internal;
public class UserPaymentResponse : BaseEntity
{
    public string? InvoiceId { get; set; }
    public DateTime Date { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }

    public Guid UserId { get; set; }
    public PaymentStatus? Status { get; set; }
}