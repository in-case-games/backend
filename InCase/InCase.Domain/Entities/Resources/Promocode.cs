﻿using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class Promocode : BaseEntity
    {
        public string? Name { get; set; }
        public int Discount { get; set; }
        public int NumberActivations { get; set; }
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore]
        public Guid TypeId { get; set; }

        public PromocodeType? Type { get; set; }

        [JsonIgnore]
        public List<UserHistoryPromocode>? History { get; set; }

        public PromocodeDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            Discount = Discount,
            NumberActivations = NumberActivations,
            ExpirationDate = ExpirationDate,
            TypeId = Type?.Id ?? TypeId
        };
    }
}
