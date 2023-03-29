using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class NewsImage: BaseEntity
    {
        public string? ImageUri { get; set; }
        [JsonIgnore]
        public Guid NewsId { get; set; }
        public News? News { get; set; }

        public NewsImageDto Convert() => new()
        {
            ImageUri = ImageUri,
            NewsId = News?.Id ?? NewsId,
        };
    }
}
