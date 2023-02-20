using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemBuyTM
    {
        [JsonPropertyName("result")]
        public string? Result { get; set; }
        [JsonPropertyName("id")]
        public int BuyId { get; set; }
    }
}
