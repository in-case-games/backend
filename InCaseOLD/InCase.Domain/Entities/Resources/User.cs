﻿using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class User : BaseEntity
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public UserAdditionalInfo? AdditionalInfo { get; set; }

        public List<SupportTopic>? Topics { get; set; }
        public List<UserRestriction>? Restrictions { get; set; }
        public List<UserRestriction>? OwnerRestrictions { get; set; }
        public List<UserReview>? Reviews { get; set; }
        public List<UserHistoryPayment>? HistoryPayments { get; set; }

        [JsonIgnore]
        public List<SupportTopicAnswer>? Answers { get; set; }
        [JsonIgnore]
        public List<UserHistoryPromocode>? HistoryPromocodes { get; set; }
        [JsonIgnore]
        public List<UserPathBanner>? Paths { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public List<UserInventory>? Inventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryWithdraw>? HistoryWithdraws { get; set; }

        public UserDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Login = Login,
            Email = Email
        };
    }
}