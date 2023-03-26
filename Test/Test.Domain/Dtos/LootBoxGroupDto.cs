﻿using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class LootBoxGroupDto : BaseEntity
    {
        public Guid BoxId { get; set; }
        public Guid GroupId { get; set; }
        public Guid GameId { get; set; }

        public LootBoxGroup Convert() => new()
        {
            BoxId = BoxId,
            GroupId = GroupId,
            GameId = GameId
        };
    }
}
