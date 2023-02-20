using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class RequestInvoiceStatusPattern
    {
        [JsonPropertyName("project")]
        public int Project { get; set; }
        [JsonPropertyName("invoice")]
        public int Invoice { get; set; }
        [JsonPropertyName("signature")]
        public string? Signature { get; set; }
    }
}
