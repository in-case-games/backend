using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class GameMarketDto : BaseEntity
    {
        public string? Name { get; set; }
        public string? DomainUri { get; set; }
        public string? ImageUri { get; set; }

        public Guid GameId { get; set; }

        public GameMarket Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            ImageUri = ImageUri,
            GameId = GameId,
            DomainUri = DomainUri,
        };
    }
}
