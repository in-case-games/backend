using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserHistoryWithdraw : BaseEntity
    {
        public int IdForMarket { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Guid StatusId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public ItemWithdrawStatus? Status { get; set; }

        public UserHistoryWithdrawDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId,
            StatusId = Status?.Id ?? StatusId,
            IdForMarket = IdForMarket
        };
    }
}
