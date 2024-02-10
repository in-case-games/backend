using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Payment.DAL.Entities;
public class PaymentStatus : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public UserPayment? Payment { get; set; }
}