using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class RequestInsertGM
    {
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("projectId")]
        public Guid PaymentId { get; set; }
        [JsonPropertyName("user")]
        public Guid UserId { get; set; }
        [JsonPropertyName("ip")]
        public string? UserIp { get; set; }
        [JsonPropertyName("amount")]
        public decimal PaymentAmount { get; set; }
        [JsonPropertyName("wallet")]
        public string? Wallet { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("rand")]
        public string? Rand { get; set;}
        [JsonPropertyName("signature")]
        public string? SignatureRSA { get; set; }

        public override string ToString()
        {
            string rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand}";
            return 
                $"project:{ProjectId};" +
                $"projectId:{PaymentId};" +
                $"user:{UserId};" +
                $"ip:{UserIp};" +
                $"amount:{PaymentAmount};" +
                $"wallet:{Wallet};" +
                $"type:{Type};" +
                $"description:{Description};" +
                $"{rand};";
        }
    }
}
