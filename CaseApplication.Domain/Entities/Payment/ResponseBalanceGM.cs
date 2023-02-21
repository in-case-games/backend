using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class ResponseBalanceGM
    {
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
        [JsonPropertyName("project_income")]
        public decimal ProjectIncome { get; set; }
        [JsonPropertyName("project_outcome")]
        public decimal ProjectOutcome { get; set; }
        [JsonPropertyName("project_balance")]
        public decimal ProjectBalance { get; set; }
        [JsonPropertyName("contract_income")]
        public decimal ContractIncome { get; set; }
        [JsonPropertyName("contract_outcome")]
        public decimal ContractOutcome { get; set; }
        [JsonPropertyName("contract_balance")]
        public decimal ContractBalance { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("rand")]
        public string? Rand { get; set; }
        [JsonPropertyName("signature")]
        public decimal SignatureRSA { get; set; }

        public override string ToString()
        {
            string rand = (string.IsNullOrEmpty(Rand)) ? $"rand:{Rand}" : "";
            string currency = (string.IsNullOrEmpty(Currency)) ? $"currency:{Currency}" : "";

            return 
                $"state:{State};" +
                $"project:{ProjectId};" +
                $"{currency};" +
                $"project_income:{ProjectIncome}" +
                $"{rand};";
        }
    }
}
