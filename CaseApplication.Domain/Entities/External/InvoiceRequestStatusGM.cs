using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class InvoiceRequestStatusGM
    {
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("invoice")]
        public int InvoiceId { get; set; }
        [JsonPropertyName("signature")]
        public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            return $"project:{ProjectId};invoice:{InvoiceId};";
        }
    }
}
