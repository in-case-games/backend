﻿using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class LootBox : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal VirtualBalance { get; set; } = 0;
        public string? Uri { get; set; } = "";
        public bool IsLocked { get; set; } = false;

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public List<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public List<LootBoxGroup>? Groups { get; set; }
        [JsonIgnore]
        public LootBoxBanner? Banner { get; set; }

        public LootBoxDto Convert() => new()
        {
            Name = Name,
            Cost = Cost,
            Balance = Balance,
            VirtualBalance = VirtualBalance,
            Uri = Uri,
            IsLocked = IsLocked,
            GameId = Game?.Id ?? GameId
        };
    }
}
