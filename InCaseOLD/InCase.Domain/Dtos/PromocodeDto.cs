﻿using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class PromocodeDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Discount { get; set; }
        public int NumberActivations { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Guid TypeId { get; set; }

        public Promocode Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            Discount = Discount,
            NumberActivations = NumberActivations,
            ExpirationDate = ExpirationDate,
            TypeId = TypeId
        };
    }
}