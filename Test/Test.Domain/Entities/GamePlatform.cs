using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class GamePlatform : BaseEntity
    {
        public string? Name { get; set; }
        public string? URI { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }

        public GamePlatformDto Convert() => new()
        {
            Name = Name,
            URI = URI,
            GameId = Game?.Id ?? GameId,
        };
    }
}
