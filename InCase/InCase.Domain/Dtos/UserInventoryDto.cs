using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserInventoryDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal FixedCost { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }

        public UserInventory Convert() => new()
        {
            Id = Id,
            Date = Date,
            FixedCost = FixedCost,
            UserId = UserId,
            ItemId = ItemId
        };
    }
}
