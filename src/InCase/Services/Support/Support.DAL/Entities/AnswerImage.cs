using System.Text.Json.Serialization;

namespace Support.DAL.Entities;
public class AnswerImage : BaseEntity
{
    [JsonIgnore]
    public Guid AnswerId { get; set; }
    [JsonIgnore]
    public SupportTopicAnswer? Answer { get; set; }
}