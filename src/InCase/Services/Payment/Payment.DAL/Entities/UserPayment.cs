using System.Text.Json.Serialization;

namespace Payment.DAL.Entities;

public class UserPayment : BaseEntity
{
    public string? InvoiceId { get; set; }
    public DateTime Date { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal Rate { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}