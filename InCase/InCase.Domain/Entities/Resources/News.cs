using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class News : BaseEntity
    {
        public string? Title { get; set; }
        public DateTime? Date { get; set; }
        public string? Content { get; set; }
        public List<NewsImage>? Images { get; set; }
    }
}
