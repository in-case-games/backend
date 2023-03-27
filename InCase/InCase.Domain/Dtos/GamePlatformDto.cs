using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class GamePlatformDto : BaseEntity
    {
        public string? Name { get; set; }
        public string? URI { get; set; }

        public Guid GameId { get; set; }

        public GamePlatform Convert() => new()
        {
            Name = Name,
            URI = URI,
            GameId = GameId
        };
    }
}
