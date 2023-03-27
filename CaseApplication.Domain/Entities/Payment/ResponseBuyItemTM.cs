﻿using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Payment
{
    public class ResponseBuyItemTM: PaymentEntity
    {
        [JsonPropertyName("result")] public string? Result { get; set; }
        [JsonPropertyName("id")] public int BuyId { get; set; }
    }
}