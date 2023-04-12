using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Payment
{
    public class BuyOfferTM : PaymentEntity
    {
        [JsonPropertyName("c")] public string? Count { get; set; }
        [JsonPropertyName("my_count")] public string? MyCount { get; set; }
        [JsonPropertyName("o_price")] public string? Price { get; set; }
    }
}
