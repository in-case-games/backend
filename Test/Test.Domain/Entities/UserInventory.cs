using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class UserInventory : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal FixedCost { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }

        public UserInventoryDto Convert() => new()
        {
            Date = Date,
            FixedCost = FixedCost,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId
        };
    }
}
