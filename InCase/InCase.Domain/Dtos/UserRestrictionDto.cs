﻿using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserRestrictionDto : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public UserRestriction Convert() => new()
        {
            Name = Name,
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Description = Description,
            UserId = UserId
        };
    }
}