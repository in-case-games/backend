using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class GamePlatform : BaseEntity
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }
        public string? ImageUri { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }

        public GamePlatformDto Convert() => new()
        {
            Name = Name,
            Uri = Uri,
            ImageUri = ImageUri,
            GameId = Game?.Id ?? GameId,
        };
    }
}
