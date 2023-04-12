using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Payment
{
    public class ResponseBalanceTM : PaymentEntity
    {
        [JsonPropertyName("money")] public decimal Money { get; set; }
    }
}
