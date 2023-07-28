using Identity.DAL.Entities;

namespace Identity.BLL.Models
{
    public class UserRestrictionResponse : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }
        public Guid? OwnerId { get; set; }

        public RestrictionTypeResponse? Type { get; set; }
    }
}
