using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Payment
{
    public class ResponsePaymentGM
    {
        [JsonPropertyName("state")] public string? StatusAnswer { get; set; }
        [JsonPropertyName("invoice")] public string? Invoice { get; set; }
        [JsonPropertyName("type")] string? TypeAnswer { get; set; }
        [JsonPropertyName("data")] public string? ParametersAnswer { get; set; }
        [JsonPropertyName("rand")] public string? Rand { get; set; }
        [JsonPropertyName("time")] public long SendTimeAnswer { get; set; }
        [JsonPropertyName("signature")] public string? SignatureRSA { get; set; }

        public override string ToString()
        {
            string rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand}";

            return
                $"state:{StatusAnswer};" +
                $"invoice:{Invoice};" +
                $"type:{TypeAnswer};" +
                $"data:{ParametersAnswer};" +
                $"{rand};" +
                $"time:{SendTimeAnswer};";
        }
    }
}
