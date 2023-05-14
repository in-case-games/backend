﻿using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Payment
{
    public class RequestInvoiceStatusGM
    {
        [JsonPropertyName("project")] public string? ProjectId { get; set; }
        [JsonPropertyName("invoice")] public string? InvoiceId { get; set; }
        [JsonPropertyName("projectId")] public string? Rand { get; set; }
        [JsonPropertyName("signature")] public string? SignatureHMAC { get; set; }

        public override string ToString()
        {
            string rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand}";
            return $"project:{ProjectId};invoice:{InvoiceId};{rand};";
        }
    }
}