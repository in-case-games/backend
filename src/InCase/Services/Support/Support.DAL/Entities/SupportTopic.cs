using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Support.DAL.Entities;
public class SupportTopic : BaseEntity
{
    [MaxLength(50)]
    public string? Title { get; set; }
    [MaxLength(120)]
    public string? Content { get; set; }
    public DateTime Date { get; set; }
    public bool IsClosed { get; set; }

    public IEnumerable<SupportTopicAnswer>? Answers { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}