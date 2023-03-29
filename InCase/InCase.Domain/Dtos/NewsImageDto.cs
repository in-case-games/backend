﻿using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class NewsImageDto : BaseEntity
    {
        public string? ImageUri { get; set; }
        public Guid NewsId { get; set; }

        public NewsImage Convert() => new()
        {
            ImageUri = ImageUri,
            NewsId = NewsId,
        };
    }
}
