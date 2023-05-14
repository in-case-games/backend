﻿using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryOpeningDto : BaseEntity
    {
        public DateTime? Date { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }

        public UserHistoryOpening Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            UserId = UserId,
            ItemId = ItemId,
            BoxId = BoxId
        };
    }
}