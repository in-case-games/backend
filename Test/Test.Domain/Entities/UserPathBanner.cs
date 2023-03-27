﻿using System.Text.Json.Serialization;

namespace Test.Domain.Entities
{
    public class UserPathBanner : BaseEntity
    {
        public DateTime Date { get; set; }
        public int NumberSteps { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BannerId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public LootBoxBanner? Banner { get; set; }

        public UserPathBanner Convert() => new()
        {
            Date = Date,
            NumberSteps = NumberSteps,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId,
            BannerId = Banner?.Id ?? BannerId
        };
    }
}
