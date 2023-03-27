﻿using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class AnswerImage : BaseEntity
    {
        public string? Uri { get; set; } = "";

        [JsonIgnore]
        public Guid AnswerId { get; set; }

        public SupportTopicAnswer? Answer { get; set; }

        public AnswerImageDto Convert() => new()
        {
            Uri = Uri,
            AnswerId = Answer?.Id ?? AnswerId
        };
    }
}
