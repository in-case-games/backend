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

        public override string ToString()
        {
            string rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand}";
            return $"project:{ProjectId};{rand};currency:{Currency};";
        }
    }
}
