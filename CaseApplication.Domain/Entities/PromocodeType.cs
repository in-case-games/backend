﻿using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities
{
    public class PromocodeType: BaseEntity
    {
        public string? PromocodeTypeName { get; set; }
        [JsonIgnore]
        public Promocode? Promocode { get; set; }
    }
}