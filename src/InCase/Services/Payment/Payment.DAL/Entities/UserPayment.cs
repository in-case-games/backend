using System.Text.Json.Serialization;

namespace Payment.DAL.Entities;

public class UserPayment : BaseEntity
{
    public string? InvoiceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public Guid StatusId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public PaymentStatus? Status { get; set; }
}