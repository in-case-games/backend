using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;

public class Recipient
{
    [JsonPropertyName("account_id")] public int AccountId { get; set; }
    [JsonPropertyName("gateway_id")] public int GatewayId { get; set; }
}