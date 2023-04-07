using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class GameItemDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public string? ImageUri { get; set; }
        public string? IdForPlatform { get; set; }

        public Guid GameId { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? RarityId { get; set; }
        public Guid? QualityId { get; set; }

        public GameItem Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            Cost = Cost,
            ImageUri = ImageUri,
            IdForPlatform = IdForPlatform,
            TypeId = TypeId,
            RarityId = RarityId,
            GameId = GameId,
            QualityId = QualityId,
        };
    }
}
