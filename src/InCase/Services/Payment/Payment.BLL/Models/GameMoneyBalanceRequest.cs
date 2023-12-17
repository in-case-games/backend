using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models
{
    public class GameMoneyBalanceRequest : IGameMoneyRequest
    {
        [JsonPropertyName("project")] public int ProjectId { get; set; }
        [JsonPropertyName("currency")] public string? Currency { get; set; }
        [JsonPropertyName("rand")] public string? Rand { get; set; }
        [JsonPropertyName("signature")] public string? SignatureHMAC { get; set; }

        public override string ToString() => string.IsNullOrEmpty(Rand) ?
            $"project:{ProjectId};currency:{Currency};" :
            $"project:{ProjectId};rand:{Rand};currency:{Currency};";
    }
}
