using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class NewsImage: BaseEntity
    {
        public string? Uri { get; set; }
        [JsonIgnore]
        public Guid NewsId { get; set; }
        public News? News { get; set; }

        public NewsImageDto Convert() => new()
        {
            Uri = Uri,
            NewsId = News?.Id ?? NewsId,
        };
    }
}
