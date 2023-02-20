using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemInfoTM
    {
        [JsonPropertyName("classid")]
        public int ClassId { get; set; }
        [JsonPropertyName("instanceid")]
        public int InstanceId { get; set; }
        [JsonPropertyName("our_market_instanceid")]
        public int? OurMarketInstanceId { get; set; }
        [JsonPropertyName("market_name")]
        public string? MarketName { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("market_hash_name")]
        public string? MarketHashName { get; set; }
        [JsonPropertyName("rarity")]
        public string? Rarity { get; set; }
        [JsonPropertyName("quality")]
        public string? Quality { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("mtype")]
        public string? MType { get; set; }
        [JsonPropertyName("slot")]
        public string? Slot { get; set; }
        [JsonPropertyName("stickers")]
        public string? Stickers { get; set; }
        [JsonPropertyName("min_price")]
        public string? MinPrice { get; set; }

        [JsonPropertyName("offers")]
        ICollection<ItemOfferTM>? Offers { get; set; }
        [JsonPropertyName("buy_offers")]
        ICollection<ItemBuyOfferTM>? BuyOffers { get; set; }
    }
}
