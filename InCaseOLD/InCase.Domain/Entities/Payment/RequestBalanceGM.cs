﻿using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Payment
{
    public class RequestBalanceGM
    {
        [JsonPropertyName("project")] public int ProjectId { get; set; }
        [JsonPropertyName("currency")] public string? Currency { get; set; }
        [JsonPropertyName("rand")] public string? Rand { get; set; }
        [JsonPropertyName("signature")] public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            string rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand}";
            return $"project:{ProjectId};{rand};currency:{Currency};";
        }
    }
}