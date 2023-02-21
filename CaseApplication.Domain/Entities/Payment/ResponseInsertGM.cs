using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class ResponseInsertGM
    {
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        [JsonPropertyName("rand")]
        public string? Rand { get; set;}
        [JsonPropertyName("signature")]
        public string? SignatureRSA { get; set;}
    }
}
