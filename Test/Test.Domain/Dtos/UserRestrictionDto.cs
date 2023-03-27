using Test.Domain.Entities;

namespace Test.Domain.Dtos
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
