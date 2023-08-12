using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models
{
    public class GameMoneyNotifySuccessRequest : IGameMoneyRequest
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            return $"success: {Success};";
        }
    }
}
