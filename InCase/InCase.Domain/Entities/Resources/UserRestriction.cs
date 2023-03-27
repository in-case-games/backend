﻿using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserRestriction : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public User? Owner { get; set; }

        public UserRestrictionDto Convert() => new()
        {
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Description = Description,
            UserId = User?.Id ?? UserId
        };
    }
}
