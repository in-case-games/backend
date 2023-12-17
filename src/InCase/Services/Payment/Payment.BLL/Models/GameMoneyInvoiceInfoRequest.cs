using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models
{
    public class GameMoneyInvoiceInfoRequest : IGameMoneyRequest
    {
        [JsonPropertyName("project")] public string? ProjectId { get; set; }
        [JsonPropertyName("invoice")] public string? InvoiceId { get; set; }
        [JsonPropertyName("projectId")] public string? Rand { get; set; }
        [JsonPropertyName("signature")] public string? SignatureHMAC { get; set; }

        public override string ToString() => string.IsNullOrEmpty(Rand) ?
            $"project:{ProjectId};invoice:{InvoiceId};" :
            $"project:{ProjectId};invoice:{InvoiceId};{Rand};";
    }
}
