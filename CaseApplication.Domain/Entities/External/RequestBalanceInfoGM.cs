using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class RequestBalanceInfoGM
    {
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
        [JsonPropertyName("signature")]
        public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            return $"project:{ProjectId};currency:{Currency};";
        }
    }
}
