using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class AnswerImage : BaseEntity
    {
        public string? ImageUri { get; set; } = "";

        [JsonIgnore]
        public Guid AnswerId { get; set; }
        [JsonIgnore]
        public SupportTopicAnswer? Answer { get; set; }
    }
}
