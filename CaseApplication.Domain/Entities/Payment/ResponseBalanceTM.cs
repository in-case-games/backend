using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class ResponseBalanceTM: PaymentEntity
    {
        [JsonPropertyName("money")] public decimal Money { get; set; }
    }
}
