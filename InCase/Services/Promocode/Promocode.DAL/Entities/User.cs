﻿using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserHistoryPromocode>? HistoriesPromocodes { get; set; }
    }
}