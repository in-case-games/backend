﻿using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class GameItem : BaseEntity
    {
        public string? IdForMarket { get; set; }
        public decimal Cost { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public List<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
    }
}