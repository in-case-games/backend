﻿using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class LootBoxGroup : BaseEntity
    {
        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }

        public GroupLootBox? Group { get; set; }
        public LootBox? Box { get; set; }
        public Game? Game { get; set; }

        public LootBoxGroupDto Convert() => new()
        {
            BoxId = Box?.Id ?? BoxId,
            GroupId = Group?.Id ?? GroupId,
            GameId = Game?.Id ?? GameId
        };
    }
}
