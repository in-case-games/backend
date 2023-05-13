using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class LootBoxGroup : BaseEntity
    {
        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }

        public GroupLootBox? Group { get; set; }
        public LootBox? Box { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }

        public LootBoxGroupDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            BoxId = Box?.Id ?? BoxId,
            GroupId = Group?.Id ?? GroupId,
            GameId = Game?.Id ?? GameId
        };
    }
}
