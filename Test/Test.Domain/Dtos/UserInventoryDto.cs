﻿using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class UserInventoryDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal FixedCost { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }

        public UserInventory Convert() => new()
        {
            Date = Date,
            FixedCost = FixedCost,
            UserId = UserId,
            ItemId = ItemId
        };
    }
}
