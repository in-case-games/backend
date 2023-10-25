using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models
{
    public class GameMoneyTopUpResponse : IGameMoneyResponse
    {
        [JsonPropertyName("status")] public string? StatusAnswer { get; set; }
        [JsonPropertyName("invoice")] public string? InvoiceId { get; set; }
        [JsonPropertyName("type")] string? TypeAnswer { get; set; }
        [JsonPropertyName("signature")] public string SignatureRSA { get; set; } = null!;

        public override string ToString()
        {
            return
                $"status:{StatusAnswer};" +
                $"invoice:{InvoiceId};" +
                $"type:{TypeAnswer};";
        }
    }
}
