﻿using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class UserHistoryOpeningDto : BaseEntity
    {
        public DateTime? Date { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }

        public UserHistoryOpening Convert() => new()
        {
            Date = Date,
            UserId = UserId,
            ItemId = ItemId,
            BoxId = BoxId
        };
    }
}
