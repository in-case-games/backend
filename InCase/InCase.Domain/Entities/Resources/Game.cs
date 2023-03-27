﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public List<GameItem>? Items { get; set; }
        public List<LootBox>? Boxes { get; set; }
        public List<GamePlatform>? Platforms { get; set; }

        [JsonIgnore]
        public List<LootBoxGroup>? LootBoxGroups { get; set; }
    }
}