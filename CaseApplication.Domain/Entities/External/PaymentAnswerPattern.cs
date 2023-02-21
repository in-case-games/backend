using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class PaymentAnswerPattern
    {
        [JsonPropertyName("state")]
        public string? StatusAnswer { get; set; }
        [JsonPropertyName("invoice")]
        public int Invoice { get; set; }
        [JsonPropertyName("type")]
        public string? TypeAnswer { get; set; }
        [JsonPropertyName("data")]
        public string? ParametersAnswer { get; set; }
        [JsonPropertyName("time")]
        public int SendTimeAnswer { get; set; }
        [JsonPropertyName("signature")]
        public string? SignatureRSA { get; set; }

        public override string ToString()
        {
            return 
                $"state:{StatusAnswer};" +
                $"invoice:{Invoice};" +
                $"type:{TypeAnswer};" +
                $"data:{ParametersAnswer};" +
                $"time:{SendTimeAnswer};";
        }
    }
}
