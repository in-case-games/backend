using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Payment.DAL.Entities;
public class UserPayment : BaseEntity
{
    [MaxLength(100)]
    public string? InvoiceId { get; set; }
    [MaxLength(100)]
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateTo { get; set; }
    public Guid UserId { get; set; }
    public Guid StatusId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public PaymentStatus? Status { get; set; }
}