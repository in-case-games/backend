using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class GamePlatformDto : BaseEntity
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }
        public string? ImageUri { get; set; }

        public Guid GameId { get; set; }

        public GamePlatform Convert() => new()
        {
            Name = Name,
            Uri = Uri,
            GameId = GameId
        };
    }
}
