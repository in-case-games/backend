using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class GameItemDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public string? Image { get; set; }
        public string? IdForPlatform { get; set; }

        public Guid GameId { get; set; }
        public Guid TypeId { get; set; }
        public Guid RarityId { get; set; }

        public GameItem Convert() => new()
        {
            Name = Name,
            Cost = Cost,
            Image = Image,
            IdForPlatform = IdForPlatform,
            TypeId = TypeId,
            RarityId = RarityId,
            GameId = GameId
        };
    }
}
