﻿using System.Text.Json.Serialization;

namespace SupportTopic.DAL.Entities
{
    public class SupportTopicAnswer : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public IEnumerable<AnswerImage>? Images { get; set; }

        [JsonIgnore]
        public Guid? PlaintiffId { get; set; }
        [JsonIgnore]
        public Guid TopicId { get; set; }
        [JsonIgnore]
        public User? Plaintiff { get; set; }
        [JsonIgnore]
        public SupportTopic? Topic { get; set; }
    }
}
