using System.Text.Json.Serialization;

namespace Payment.BLL.Interfaces
{
    public interface IGameMoneyResponse
    {
        [JsonPropertyName("signature")] public string SignatureRSA { get; set; }
        public string ToString();
    }
}
