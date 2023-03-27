using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class NewsImage: BaseEntity
    {
        public string? ImageUri { get; set; }
        [JsonIgnore]
        public int NewsId { get; set; }
        public News? News { get; set; }
    }
}
