﻿using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemBuyOfferTM
    {
        [JsonPropertyName("c")]
        public string? Count { get; set; }
        [JsonPropertyName("my_count")]
        public string? MyCount { get; set; }
        [JsonPropertyName("o_price")]
        public string? Price { get; set; }
    }
}
