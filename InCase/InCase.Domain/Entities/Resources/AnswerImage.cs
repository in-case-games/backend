﻿using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class AnswerImage : BaseEntity
    {
        public string? ImageUri { get; set; } = "";

        [JsonIgnore]
        public Guid AnswerId { get; set; }

        public SupportTopicAnswer? Answer { get; set; }
    }
}
