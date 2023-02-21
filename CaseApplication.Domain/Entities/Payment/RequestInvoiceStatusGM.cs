using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class RequestInvoiceStatusGM
    {
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("invoice")]
        public int InvoiceId { get; set; }
        [JsonPropertyName("projectId")]
        public string? Rand { get; set; }
        [JsonPropertyName("signature")]
        public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            string rand = (string.IsNullOrEmpty(Rand)) ? "" : $"rand:{Rand}";
            return $"project:{ProjectId};invoice:{InvoiceId};{rand};";
        }
    }
}
