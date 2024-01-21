using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models; 

public class GameMoneyNotifySuccessRequest : IGameMoneyRequest
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    public string? SignatureHmac { get; set; }

    public override string ToString() => $"success: {Success};";
}