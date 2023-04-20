using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryWithdrawDto : BaseEntity
    {
        public int IdForMarket { get; set; }
        public DateTime Date { get; set; }
        public Guid MarketId { get; set; }
        public Guid StatusId { get; set; }
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }

        public UserHistoryWithdraw Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            StatusId = StatusId,
            UserId = UserId,
            ItemId = ItemId,
            MarketId = MarketId,
            IdForMarket = IdForMarket
        };
    }
}
