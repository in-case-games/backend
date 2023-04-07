using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserHistoryWithdrawn : BaseEntity
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }

        public UserHistoryWithdrawnDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId
        };
    }
}
