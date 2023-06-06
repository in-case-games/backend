using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class Promocode : BaseEntity
    {
        public decimal Discount { get; set; }

        [JsonIgnore]
        public IEnumerable<UserHistoryPromocode>? HistoryPromocodes { get; set; }
    }
}
