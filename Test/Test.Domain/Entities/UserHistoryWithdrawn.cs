using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
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

        public UserHistoryWithdrawnDto Convert() => new()
        {
            Date = Date,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId
        };
    }
}
