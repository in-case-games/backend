using System.ComponentModel;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemInfoTM
    {
        [DisplayName("classid")]
        public int ClassId { get; set; }
        [DisplayName("instanceid")]
        public int InstanceId { get; set; }
        [DisplayName("our_market_instanceid")]
        public int? OurMarketInstanceId { get; set; }
        [DisplayName("market_name")]
        public string? MarketName { get; set; }
        [DisplayName("name")]
        public string? Name { get; set; }
        [DisplayName("market_hash_name")]
        public string? MarketHashName { get; set; }
        [DisplayName("rarity")]
        public string? Rarity { get; set; }
        [DisplayName("quality")]
        public string? Quality { get; set; }
        [DisplayName("type")]
        public string? Type { get; set; }
        [DisplayName("mtype")]
        public string? MType { get; set; }
        [DisplayName("slot")]
        public string? Slot { get; set; }
        [DisplayName("stickers")]
        public string? Stickers { get; set; }
        [DisplayName("min_price")]
        public string? MinPrice { get; set; }

        [DisplayName("offers")]
        ICollection<ItemOfferTM>? Offers { get; set; }
        [DisplayName("buy_offers")]
        ICollection<ItemBuyOfferTM>? BuyOffers { get; set; }
    }
}
