﻿using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class UserHistoryPayment : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserHistoryPaymentDto Convert() => new()
        {
            Date = Date,
            Amount = Amount,
            UserId = User?.Id ?? UserId
        };
    }
}
