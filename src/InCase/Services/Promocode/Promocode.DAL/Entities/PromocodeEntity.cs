﻿using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities
{
    public class PromocodeEntity : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Discount { get; set; }
        public int NumberActivations { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PromocodeType? Type { get; set; }

        [JsonIgnore]
        public Guid TypeId { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPromocode>? HistoriesPromocodes { get; set; }
    }
}
