using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemOfferTM
    {
        [JsonPropertyName("price")]
        public string? Price { get; set; }
        [JsonPropertyName("count")]
        public string? Count { get; set; }
        [JsonPropertyName("my_count")]
        public string? MyCount { get; set; }
    }
}
