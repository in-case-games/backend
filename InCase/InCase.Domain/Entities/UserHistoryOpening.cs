using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities
{
    public class UserHistoryOpening : BaseEntity
    {
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public LootBox? Box { get; set; }

        public UserHistoryOpeningDto Convert() => new()
        {
            Date = Date,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId,
            BoxId = Box?.Id ?? BoxId
        };
    }
}
