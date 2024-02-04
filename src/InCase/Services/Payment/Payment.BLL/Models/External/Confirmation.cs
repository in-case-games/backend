using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;

public class Confirmation
{
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("return_url")] public string? ReturnUrl { get; set; }
    [JsonPropertyName("confirmation_url")] public string? ConfirmationUrl { get; set; }
}