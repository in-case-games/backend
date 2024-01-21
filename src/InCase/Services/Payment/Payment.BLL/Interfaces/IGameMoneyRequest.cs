using System.Text.Json.Serialization;

namespace Payment.BLL.Interfaces;

public interface IGameMoneyRequest
{
    [JsonPropertyName("signature")] public string? SignatureHmac { get; set; }
    public string ToString();
}