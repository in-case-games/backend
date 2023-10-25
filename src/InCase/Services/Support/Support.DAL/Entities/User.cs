using System.Text.Json.Serialization;

namespace Support.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<SupportTopic>? Topics { get; set; }
        [JsonIgnore]
        public IEnumerable<SupportTopicAnswer>? Answers { get; set; }
    }
}
