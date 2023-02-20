﻿using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Internal
{
    public class CaseInventory : BaseEntity
    {
        [JsonIgnore]
        public Guid GameCaseId { get; set; }
        [JsonIgnore]
        public Guid GameItemId { get; set; }
        public int NumberItemsCase { get; set; }
        public int LossChance { get; set; }
        [JsonIgnore]
        public GameCase? GameCase { get; set; }
        public GameItem? GameItem { get; set; }

    }
}