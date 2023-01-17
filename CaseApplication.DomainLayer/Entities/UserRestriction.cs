﻿namespace CaseApplication.DomainLayer.Entities
{
    public class UserRestriction : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? RestrictionName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}