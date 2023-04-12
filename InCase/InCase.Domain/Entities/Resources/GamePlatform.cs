using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class GamePlatform : BaseEntity
    {
        public string? Name { get; set; }
        public string? DomainUri { get; set; }
        public string? ImageUri { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }

        public GamePlatformDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            ImageUri = ImageUri,
            DomainUri = DomainUri,
            GameId = Game?.Id ?? GameId,
        };
    }
}
