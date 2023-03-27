﻿using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;
using System.Text.Json.Serialization;

namespace InCase.Domain.Dtos
{
    public class AnswerImageDto : BaseEntity
    {
        public string? Uri { get; set; } = "";

        public Guid AnswerId { get; set; }

        public AnswerImage Convert() => new()
        {
            Uri = Uri,
            AnswerId = AnswerId
        };
    }
}
