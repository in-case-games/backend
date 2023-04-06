using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserRestrictionDto : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid TypeId { get; set; }

        public UserRestriction Convert() => new()
        {
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Description = Description,
            UserId = UserId,
            OwnerId = OwnerId,
            TypeId = TypeId
        };
    }
}
