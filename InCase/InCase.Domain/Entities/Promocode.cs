using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities
{
    public class Promocode : BaseEntity
    {
        public string? Name { get; set; }
        public string? Discount { get; set; }
        public int NumberActivations { get; set; }
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore]
        public Guid TypeId { get; set; }

        public PromocodeType? Type { get; set; }

        [JsonIgnore]
        public List<UserHistoryPromocode>? History { get; set; }

        public PromocodeDto Convert() => new()
        {
            Name = Name,
            Discount = Discount,
            NumberActivations = NumberActivations,
            ExpirationDate = ExpirationDate,
            TypeId = Type?.Id ?? TypeId
        };
    }
}
